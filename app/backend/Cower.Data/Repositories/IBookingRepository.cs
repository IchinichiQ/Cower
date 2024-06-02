using Cower.Data.Models;
using Cower.Domain.Models.Booking;

namespace Cower.Data.Repositories;

public interface IBookingRepository
{
    Task<IReadOnlyCollection<BookingTimeSlotDal>> GetBookingsTimeSlots(
        DateOnly date,
        long floorId);
    Task<IReadOnlyCollection<BookingDal>> GetUserBookings(long userId);
    Task<BookingDal?> GetBooking(long id);
    Task<BookingDal?> GetBooking(string label);
    Task<IReadOnlyCollection<BookingDal>> GetBookings();
    Task<BookingDal> AddBooking(BookingDal booking);
    Task<bool> IsBookingTimeOverlaps(
        long seatId,
        DateOnly bookingDate,
        TimeOnly startTime,
        TimeOnly endTime);
    Task<BookingDal?> SetBookingStatus(long id, BookingStatus status);
    Task<int> SetPaymentTimeoutStatus();
    Task<int> SetInProgressStatus();
    Task<int> SetSuccessBookingStatus();
}