using Cower.Domain.Models.Booking;

namespace Cower.Service.Services;

public interface IBookingService
{
    Task<Booking?> GetBooking(long id, long userId);
    Task<IReadOnlyCollection<Booking>> GetUserBookings(long userId);
    Task<Booking> AddBooking(Booking booking);
}