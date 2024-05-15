using System.Text.Json;
using System.Text.Json.Serialization;
using Cower.Service.Services;
using Cower.Web.Extensions;
using Cower.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

[Route("api/v1/yoomoney")]
public class YoomoneyController : ControllerBase
{
    private readonly ILogger<YoomoneyController> _logger;
    private readonly IBookingService _bookingService;
    private readonly IYoomoneyService _yoomoneyService;

    public YoomoneyController(
        ILogger<YoomoneyController> logger,
        IBookingService bookingService,
        IYoomoneyService yoomoneyService)
    {
        _logger = logger;
        _bookingService = bookingService;
        _yoomoneyService = yoomoneyService;
    }
    
    [HttpPost]
    public async Task<ActionResult> ProcessNotification([FromForm] YoomoneyNotificationRequestDto notification)
    {
        _logger.LogInformation(JsonSerializer.Serialize(notification));
        
        if (!_yoomoneyService.ValidateNotification(notification.ToYoomoneyNotification()))
        {
            _logger.LogWarning("Invalid SHA-1 hash in YooMoney notification");

            var error = new ErrorDTO(
                ErrorCodes.INVALID_REQUEST_DATA,
                "Invalid SHA-1 hash");
            return BadRequest(error);
        }

        _logger.LogInformation($"Got yoomoney notification for label={notification.label}");
        
        var res = await _bookingService.ProcessPayment(notification.label, notification.amount);
        
        _logger.LogInformation($"Is notification changed status - {res}");
        
        return Ok();
    }
}