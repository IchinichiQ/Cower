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

[Route("api/v1/coworkings")]
public class CoworkingController : ControllerBase
{
    private readonly ILogger<CoworkingController> _logger;
    private readonly ICoworkingService _coworkingService;

    public CoworkingController(
        ILogger<CoworkingController> logger,
        ICoworkingService coworkingService)
    {
        _logger = logger;
        _coworkingService = coworkingService;
    }
    
    [HttpGet]
    public async Task<ActionResult<CoworkingsInfoResponseDto>> AllCoworkings()
    {
        var coworkings = await _coworkingService.GetAllCoworkings();

        return new CoworkingsInfoResponseDto
        {
            Coworkings = coworkings.Select(x => new CoworkingInfoDto
            {
                Id = x.Id,
                Address = x.Address,
                Floors = x.Floors.Select(x => x.ToCoworkingFloorInfoDto()).ToArray(),
                WorkingTime = x.WorkingTime.Select(x => x.ToCoworkingWorkingTimeDto()).ToArray()
            }).ToArray()
        };
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CoworkingDto>> CoworkingInfo([FromRoute] long id)
    {
        var coworking = await _coworkingService.GetCoworking(id);
        if (coworking == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Коворкинга с таким id не существует");
            return NotFound(error);
        }

        return Ok(coworking.ToCoworkingDto());
    }
    
    [HttpPost]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<CoworkingDto>> CreateCoworking([FromBody] CreateCoworkingDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        var requestBl = new CreateCoworkingBl(
            dto.Address,
            dto.WorkingTimes
                .Select(x => new CreateCoworkingWorkingTimeBl(
                    Enum.Parse<DayOfWeek>(x.Day),
                    TimeOnly.Parse(x.Open),
                    TimeOnly.Parse(x.Close)))
                .ToArray());

        Coworking coworking = await _coworkingService.CreateCoworking(requestBl);
        
        return Ok(coworking.ToCoworkingDto());
    }
    
    [HttpPatch("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> UpdateCoworking(
        [FromRoute] long id,
        [FromBody] UpdateCoworkingDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var bl = new UpdateCoworkingBl(
                id,
                dto.Address,
                dto.WorkingTimes?.Select(x => new UpdateCoworkingWorkingTimeBl(
                    Enum.Parse<DayOfWeek>(x.Day),
                    TimeOnly.Parse(x.Open),
                    TimeOnly.Parse(x.Close)))
                    .ToArray());

        var coworking = await _coworkingService.UpdateCoworking(bl);
        
        if (coworking == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Коворкинга с таким ID не существует");
            return NotFound(error);
        }
        
        return Ok(coworking);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> DeleteSeat([FromRoute] long id)
    {
        var isDeleted = await _coworkingService.DeleteCoworking(id);

        if (!isDeleted)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Коворкинга с таким ID не существует");
            return NotFound(error);
        }
        
        return NoContent();
    }
}