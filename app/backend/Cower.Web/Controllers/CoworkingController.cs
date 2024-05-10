using Cower.Service.Services;
using Cower.Web.Helpers;
using Cower.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

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
    
    [HttpGet("api/coworking/{id}")]
    public async Task<ActionResult<CoworkingResponseDTO>> CoworkingInfo([FromRoute] long id)
    {
        var coworking = await _coworkingService.GetCoworking(id);
        if (coworking == null)
        {
            var error = new ErrorDTO(
                ErrorCodes.NOT_FOUND,
                "Коворкинга с таким id не существует");
            return NotFound(error);
        }

        return new CoworkingResponseDTO
        {
            CoworkingId = coworking.Id,
            Address = coworking.Address,
            Floors = coworking.Floors,
            WorkingTime = coworking.WorkingTime
                .Select(x => new CoworkingWorkingTimeResponseDTO
                {
                    Day = x.Day.ToString(),
                    Open = x.Open.ToString(),
                    Close = x.Close.ToString()
                }).ToArray()
        };
    }

    [HttpGet("api/coworking/{id}/floor/{num}")]
    public async Task<ActionResult<CoworkingFloorResponseDTO>> GetCoworkingFloor([FromRoute] long id, [FromRoute] int num)
    {
        var floor = await _coworkingService.GetCoworkingFloor(id, num);
        if (floor == null)
        {
            var error = new ErrorDTO(
                ErrorCodes.NOT_FOUND,
                "Такого коворкинга или этажа не существует");
            return NotFound(error);
        }

        return new CoworkingFloorResponseDTO
        {
            CoworkingId = floor.CoworkingId,
            Floor = floor.Floor,
            BackgroundImage = floor.BackgroundFilename,
            Seats = floor.Seats
                .Select(x => new CoworkingSeatResponseDTO
                {
                    Id = x.Id,
                    CoworkingId = x.CoworkingId,
                    Floor = x.Floor,
                    Number = x.Number,
                    Price = x.Price,
                    Description = x.Description,
                    Image = x.ImageFilename,
                    Position = new CoworkingSeatPositionResponseDTO
                    {
                        X = x.Position.X,
                        Y = x.Position.Y,
                        Width = x.Position.Width,
                        Height = x.Position.Height,
                        Angle = x.Position.Angle
                    }
                }).ToArray()
        };
    }
    
    [HttpGet("api/coworking/{id}/seat/availability")]
    public async Task<ActionResult<CoworkingSeatAvailabiltiyResponseDTO>> GetSeatsAvailability(
        [FromRoute] long id,
        [FromQuery] CoworkingSeatAvailabilityRequestDTO requestDto)
    {
        var validationError = ValidationHelper.Validate(requestDto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        var availability = await _coworkingService.GetSeatsAvailability(
            DateOnly.Parse(requestDto.Date),
            id,
            requestDto.SeatIds);
        if (availability == null)
        {
            var error = new ErrorDTO(
                ErrorCodes.NOT_FOUND,
                "Такого коворкинга не существует");
            return NotFound(error);
        }
        
        return new CoworkingSeatAvailabiltiyResponseDTO
        {
            Date = availability.Date.ToString("yyyy-MM-dd"),
            Availability = availability.Availability.ToDictionary(
                x => x.Key,
                x => x.Value.Select(x => new CoworkingSeatTimeSlotResponseDTO
                {
                    From = x.From.ToString(),
                    To = x.To.ToString()
                }).ToList())
        };
    }
}