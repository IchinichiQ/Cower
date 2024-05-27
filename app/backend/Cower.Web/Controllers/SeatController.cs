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

[Route("api/v1/seats")]
public class SeatController : ControllerBase
{
    private readonly ILogger<SeatController> _logger;
    private readonly ISeatService _seatService;
    
    public SeatController(
        ILogger<SeatController> logger,
        ISeatService seatService)
    {
        _logger = logger;
        _seatService = seatService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CoworkingSeatDto>> GetSeat([FromRoute] long id)
    {
        var seat = await _seatService.GetSeat(id);
        if (seat == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Рабочего места с таким id не существует");
            return NotFound(error);
        }
        
        return Ok(seat.ToCoworkingSeatDto());
    }

    [HttpGet]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<CoworkingSeatsDto>> GetSeats()
    {
        var seats = await _seatService.GetSeats();

        return Ok(new CoworkingSeatsDto()
        {
            Seats = seats
                .Select(x => x.ToCoworkingSeatDto())
                .ToArray()
        });
    }
    
    [HttpPost]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<CoworkingFloorDto>> CreateSeat([FromBody] CreateCoworkingSeatDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        var requestBl = new CreateSeatBl(
            dto.FloorId,
            dto.Number,
            dto.Price,
            dto.Description,
            dto.ImageId,
            dto.Position.X,
            dto.Position.Y,
            dto.Position.Width,
            dto.Position.Height,
            dto.Position.Angle);

        CoworkingSeat seat;
        try
        {
            seat = await _seatService.CreateSeat(requestBl);
        }
        catch (SeatNumberExistOnFloorException)
        {
            var error = new ErrorDto(
                ErrorCodes.SEAT_NUMBER_EXIST_ON_FLOOR,
                "Место с таким номером уже существует на данном этаже");
            return BadRequest(error);
        }
        catch (FloorDoesntExistException)
        {
            var error = new ErrorDto(
                ErrorCodes.FLOOR_DOESNT_EXIST,
                "Этажа с таким ID не существует");
            return BadRequest(error);
        }
        catch (ImageDoesntExistException)
        {
            var error = new ErrorDto(
                ErrorCodes.IMAGE_DOESNT_EXIST,
                "Изображения с таким ID не существует");
            return BadRequest(error);
        }
        catch (WrongImageType)
        {
            var error = new ErrorDto(
                ErrorCodes.WRONG_IMAGE_TYPE,
                "Изображение должно иметь тип seat");
            return BadRequest(error);
        }
        
        return Ok(seat);
    }
    
    [HttpPatch("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> UpdateSeat(
        [FromRoute] long id,
        [FromBody] UpdateCoworkingSeatDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var bl = new UpdateSeatBl(
                id,
                dto.FloorId,
                dto.Number,
                dto.Price,
                dto.Description,
                dto.ImageId,
                dto.X,
                dto.Y,
                dto.Width,
                dto.Height,
                dto.Angle);

        CoworkingSeat? seat;
        try
        {
            seat = await _seatService.UpdateSeat(bl);
        }
        catch (SeatNumberExistOnFloorException)
        {
            var error = new ErrorDto(
                ErrorCodes.SEAT_NUMBER_EXIST_ON_FLOOR,
                "Место с таким номером уже существует на данном этаже");
            return BadRequest(error);
        }
        catch (FloorDoesntExistException)
        {
            var error = new ErrorDto(
                ErrorCodes.FLOOR_DOESNT_EXIST,
                "Этажа с таким ID не существует");
            return BadRequest(error);
        }
        catch (ImageDoesntExistException)
        {
            var error = new ErrorDto(
                ErrorCodes.IMAGE_DOESNT_EXIST,
                "Изображения с таким ID не существует");
            return BadRequest(error);
        }
        catch (WrongImageType)
        {
            var error = new ErrorDto(
                ErrorCodes.WRONG_IMAGE_TYPE,
                "Изображение должно иметь тип seat");
            return BadRequest(error);
        }
        
        if (seat == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Места с таким ID не существует");
            return NotFound(error);
        }
        
        return Ok(seat);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> DeleteSeat([FromRoute] long id)
    {
        var isDeleted = await _seatService.DeleteSeat(id);

        if (!isDeleted)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Места с таким ID не существует");
            return NotFound(error);
        }
        
        return NoContent();
    }
}