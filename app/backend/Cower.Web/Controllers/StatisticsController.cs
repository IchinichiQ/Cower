using Cower.Domain.Models;
using Cower.Domain.Models.Booking;
using Cower.Domain.Models.Statistics;
using Cower.Service.Exceptions;
using Cower.Service.Services;
using Cower.Web.Helpers;
using Cower.Web.Models;
using Cower.Web.Models.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

[Route("api/v1/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly ILogger<StatisticsController> _logger;
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(
        ILogger<StatisticsController> logger,
        IStatisticsService statisticsService)
    {
        _logger = logger;
        _statisticsService = statisticsService;
    }

    [HttpGet("bookings-count")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<BookingsCountStatisticsResponseDto>> GetBookingsCountStatistic(
        [FromQuery] StatisticsRequestDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        BookingsCountStatistic statistics;
        try
        {
            statistics = await _statisticsService.GetBookingsCountStatistic(
                DateOnly.Parse(dto.StartDate),
                DateOnly.Parse(dto.EndDate),
                dto.Step);
        }
        catch (BusinessLogicException e)
        {
            var error = new ErrorDto(
                ErrorCodes.INVALID_REQUEST_DATA,
                e.Message);
            return BadRequest(error);
        }
        
        return Ok(new BookingsCountStatisticsResponseDto
        {
            StartDate = statistics.StartDate.ToString("yyyy-MM-dd"),
            EndDate = statistics.EndDate.ToString("yyyy-MM-dd"),
            Step = statistics.Step,
            Values = statistics.Values
        });
    }
    
    [HttpGet("bookings-price")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<BookingsPriceStatisticsResponseDto>> GetBookingsPriceStatistic(
        [FromQuery] StatisticsRequestDto dto)
    {
        var validationError = ValidationHelper.Validate(dto);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        BookingsPriceStatistic statistics;
        try
        {
            statistics = await _statisticsService.GetBookingsPriceStatistic(
                DateOnly.Parse(dto.StartDate),
                DateOnly.Parse(dto.EndDate),
                dto.Step);
        }
        catch (BusinessLogicException e)
        {
            var error = new ErrorDto(
                ErrorCodes.INVALID_REQUEST_DATA,
                e.Message);
            return BadRequest(error);
        }
        
        return Ok(new BookingsPriceStatisticsResponseDto
        {
            StartDate = statistics.StartDate.ToString("yyyy-MM-dd"),
            EndDate = statistics.EndDate.ToString("yyyy-MM-dd"),
            Step = statistics.Step,
            Values = statistics.Values
        });
    }
}