using Cower.Data.Models;
using Cower.Domain.Models.Booking;

namespace Cower.Data.Repositories;

public interface IBookingRepository
{
    Task<IReadOnlyCollection<BookingTimeSlotDAL>> GetBookingsTimeSlots(
        DateOnly date,
        long coworkingId,
        IReadOnlyCollection<long> seatIds);
    Task<IReadOnlyCollection<BookingDAL>> GetUserBookings(long userId);
    Task<BookingDAL?> GetBooking(long id);
    Task<BookingDAL?> GetBooking(string label);
    Task<BookingDAL> AddBooking(BookingDAL booking);
    Task<bool> IsBookingTimeOverlaps(
        long seatId,
        DateOnly bookingDate,
        TimeOnly startTime,
        TimeOnly endTime);
    Task<BookingDAL?> SetBookingStatus(long id, BookingStatus status);
}