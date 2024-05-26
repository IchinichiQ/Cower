using System.ComponentModel.DataAnnotations;
using Cower.Data.Models;
using Cower.Domain.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Exceptions;
using Cower.Service.Models;
using Cower.Service.Services;
using Cower.Web.Extensions;
using Cower.Web.Helpers;
using Cower.Web.Models;
using Cower.Web.Models.Coworking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

[Route("api/v1/floors")]
public class FloorController : ControllerBase
{
    private readonly ILogger<FloorController> _logger;
    private readonly IFloorService _floorService;

    public FloorController(
        ILogger<FloorController> logger,
        IFloorService floorService)
    {
        _logger = logger;
        _floorService = floorService;
    }
    
    [HttpGet]
    public async Task<ActionResult<CoworkingFloorsInfoDto>> GetFloors()
    {
        var floors = await _floorService.GetFloors();

        return Ok(new CoworkingFloorsInfoDto
        {
            Floors = floors
                .Select(x => x.ToCoworkingFloorInfoDto())
                .ToArray()
        });
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CoworkingFloorDto>> GetFloor([FromRoute] long id)
    {
        var floor = await _floorService.GetFloor(id);
        if (floor == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Этажа с таким ID не существует");
            return NotFound(error);
        }
        
        return Ok(floor.ToCoworkingFloorDto());
    }
    
    [HttpGet("{id}/availability")]
    public async Task<ActionResult<CoworkingFloorAvailabiltiyDto>> GetFloorAvailability(
        [FromRoute] long id,
        [FromQuery] GetCoworkingFloorAvailabilityDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        var availability = await _floorService.GetFloorAvailability(
            DateOnly.Parse(dto.Date),
            id);
        if (availability == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Этажа с таким ID не существует");
            return NotFound(error);
        }
        
        return new CoworkingFloorAvailabiltiyDto
        {
            Date = availability.Date.ToString("yyyy-MM-dd"),
            Availability = availability.Availability.ToDictionary(
                x => x.Key,
                x => x.Value.Select(x => new CoworkingSeatTimeSlotResponseDto
                {
                    From = x.From.ToString(),
                    To = x.To.ToString()
                }).ToList())
        };
    }
    
    [HttpPost]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<CoworkingFloorDto>> CreateFloor(CreateCoworkingFloorDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        var requestBl = new CreateFloorBl(
            dto.CoworkingId,
            dto.ImageId,
            dto.Number);

        CoworkingFloor floor;
        try
        {
            floor = await _floorService.CreateFloor(requestBl);
        }
        catch (FloorNumberExistInCoworkingException)
        {
            var error = new ErrorDto(
                ErrorCodes.FLOOR_NUMBER_EXIST_IN_COWORKING,
                "Этаж с таким номером уже существует в данном коворкинге");
            return BadRequest(error);
        }
        catch (CoworkingDoesntExistException)
        {
            var error = new ErrorDto(
                ErrorCodes.COWORKING_DOESNT_EXIST,
                "Коворкинга с таким ID не существует");
            return BadRequest(error);
        }
        // TODO: Image id error
        
        return Ok(floor);
    }
    
    [HttpPatch("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> UpdateFloor(
        [FromRoute] long id,
        [FromBody] UpdateCoworkingFloorDal dto)
    {
        var bl = new UpdateFloorBl(
            id,
            dto.CoworkingId,
            dto.ImageId,
            dto.Number);

        CoworkingFloor? floor;
        try
        {
            floor = await _floorService.UpdateFloor(bl);
        }
        catch (FloorNumberExistInCoworkingException)
        {
            var error = new ErrorDto(
                ErrorCodes.FLOOR_NUMBER_EXIST_IN_COWORKING,
                "Этаж с таким номером уже существует в данном коворкинге");
            return BadRequest(error);
        }
        catch (CoworkingDoesntExistException)
        {
            var error = new ErrorDto(
                ErrorCodes.COWORKING_DOESNT_EXIST,
                "Коворкинга с таким ID не существует");
            return BadRequest(error);
        }
        // TODO: Image id error
        
        if (floor == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Этажа с таким ID не существует");
            return NotFound(error);
        }
        
        return Ok(floor);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> DeleteFloor([FromRoute] long id)
    {
        var isDeleted = await _floorService.DeleteFloor(id);
        if (!isDeleted)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Этажа с таким ID не существует");
            return NotFound(error);
        }
        
        return NoContent();
    }
}