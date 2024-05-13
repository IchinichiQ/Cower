using Cower.Service.Services;
using Cower.Web.Extensions;
using Cower.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

[Route("api/v1/bookings")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingService _bookingService;

    public BookingController(
        ILogger<BookingController> logger,
        IBookingService bookingService)
    {
        _logger = logger;
        _bookingService = bookingService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponseDTO>> GetBooking([FromRoute] long id)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;
        
        var booking = await _bookingService.GetBooking(id, long.Parse(userId));
        if (booking == null)
        {
            var error = new ErrorDTO(
                ErrorCodes.NOT_FOUND,
                "Бронирования с таким id не существует");
            return NotFound(error);
        }

        return new BookingResponseDTO
        {
            Booking = booking.ToBookingDTO()
        };
    }

    [HttpGet]
    public async Task<ActionResult<BookingsResponseDTO>> GetUserBookings()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

        var bookings = await _bookingService.GetUserBookings(long.Parse(userId));

        return new BookingsResponseDTO
        {
            Bookings = bookings
                .Select(x => x.ToBookingDTO())
                .ToArray()
        };
    }
}