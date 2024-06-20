namespace Cower.Service.Models;

public sealed record CreateBookingRequestBL(
    long UserId,
    string UserRole,
    long SeatId,
    bool ApplyDiscount,
    DateOnly BookingDate,
    TimeOnly StartTime,
    TimeOnly EndTime);