using Cower.Service.Services;
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
}