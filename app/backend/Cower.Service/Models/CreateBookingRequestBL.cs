namespace Cower.Service.Models;

public sealed record CreateBookingRequestBL(
    long UserId,
    long SeatId,
    DateOnly BookingDate,
    TimeOnly StartTime,
    TimeOnly EndTime);