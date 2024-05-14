using Cower.Data.Extensions;
using Cower.Data.Repositories;
using Cower.Domain.Models.Booking;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class BookingService : IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly IBookingRepository _bookingRepository;

    public BookingService(
        ILogger<BookingService> logger,
        IBookingRepository bookingRepository)
    {
        _logger = logger;
        _bookingRepository = bookingRepository;
    }
    
    public async Task<Booking?> GetBooking(long id, long userId)
    {
        var bookingDal = await _bookingRepository.GetBooking(id);
        if (bookingDal != null && bookingDal.UserId != userId)
        {
            throw new ForbiddenException();
        }
        
        return bookingDal?.ToBooking();
    }

    public async Task<IReadOnlyCollection<Booking>> GetUserBookings(long userId)
    {
        var bookingDals = await _bookingRepository.GetUserBookings(userId);

        return bookingDals
            .Select(x => x.ToBooking())
            .ToArray();
    }

    public async Task<Booking> AddBooking(Booking booking)
    {
        var bookingDal = await _bookingRepository.AddBooking(booking.ToBookingDAL());
        
        return bookingDal.ToBooking();
    }
}