using Cower.Data.Models;

namespace Cower.Data.Repositories;

public interface IBookingRepository
{
    Task<IReadOnlyCollection<BookingTimeSlotDAL>> GetBookingsTimeSlots(
        DateOnly date,
        long coworkingId,
        IReadOnlyCollection<long> seatIds);
    Task<IReadOnlyCollection<BookingDAL>> GetUserBookings(long userId);
    Task<BookingDAL?> GetBooking(long id);
    Task<BookingDAL> AddBooking(BookingDAL booking);
    Task<bool> IsBookingTimeOverlaps(
        long seatId,
        DateOnly bookingDate,
        TimeOnly startTime,
        TimeOnly endTime);
}