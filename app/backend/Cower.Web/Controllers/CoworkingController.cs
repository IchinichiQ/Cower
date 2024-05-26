using Cower.Domain.Models.Coworking;
using Cower.Service.Services;
using Cower.Web.Extensions;
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
    
    [HttpGet("api/coworking")]
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
    
    [HttpGet("api/coworking/{id}")]
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

        return new CoworkingDto
        {
            Id = coworking.Id,
            Address = coworking.Address,
            Floors = coworking.Floors.Select(x => x.ToCoworkingFloorDto()).ToArray(),
            WorkingTime = coworking.WorkingTime
                .Select(x => new CoworkingWorkingTimeDto
                {
                    Day = x.Day.ToString(),
                    Open = x.Open.ToString(),
                    Close = x.Close.ToString()
                }).ToArray()
        };
    }
}