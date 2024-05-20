using Cower.Domain.Models.Booking;
using Cower.Service.Exceptions;
using Cower.Service.Models;
using Cower.Service.Services;
using Cower.Web.Extensions;
using Cower.Web.Helpers;
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

        Booking? booking;
        try
        {
            booking = await _bookingService.GetBooking(id, long.Parse(userId));
        }
        catch (ForbiddenException)
        {
            var error = new ErrorDTO(
                ErrorCodes.FORBIDDEN,
                "Нет прав на просмотр этого бронирования");
            return new ObjectResult(error) { StatusCode = 403};
        }
        
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
    
    [HttpPost]
    public async Task<ActionResult<CreateBookingResponseDTO>> CreateBooking([FromBody] CreateBookingRequestDTO request)
    {
        var validationError = ValidationHelper.Validate(request);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

        Booking booking;
        try
        {
            booking = await _bookingService.AddBooking(new CreateBookingRequestBL(
                long.Parse(userId),
                request.SeatId,
                DateOnly.Parse(request.BookingDate),
                TimeOnly.Parse(request.StartTime),
                TimeOnly.Parse(request.EndTime))
            );
        }
        catch (NotFoundException)
        {
            var error = new ErrorDTO(
                ErrorCodes.NOT_FOUND,
                "Место с таким Id не найдено");
            return NotFound(error);
        }
        catch (BusinessLogicException e)
        {
            var error = new ErrorDTO(
                ErrorCodes.INVALID_REQUEST_DATA,
                e.Message);
            return BadRequest(error);
        }

        return new CreateBookingResponseDTO
        {
            Booking = booking.ToBookingDTO()
        };
    }
    
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<CancelBookingResponseDTO>> CancelBooking([FromRoute] long id)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

        Booking? booking;
        try
        {
            booking = await _bookingService.CancelBooking(id, long.Parse(userId));
        }
        catch (ForbiddenException)
        {
            var error = new ErrorDTO(
                ErrorCodes.FORBIDDEN,
                "Нет прав на отмену этого бронирования");
            return new ObjectResult(error) { StatusCode = 403};
        }
        catch (BusinessLogicException e)
        {
            var error = new ErrorDTO(
                ErrorCodes.INVALID_REQUEST_DATA,
                e.Message);
            return BadRequest(error);
        }

        if (booking == null)
        {
            var error = new ErrorDTO(
                ErrorCodes.NOT_FOUND,
                "Бронирования с таким id не существует");
            return NotFound(error);
        }
        
        return new CancelBookingResponseDTO
        {
            Booking = booking.ToBookingDTO()
        };
    }
}