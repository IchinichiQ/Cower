using Cower.Domain.Models.Booking;

namespace Cower.Data.Models;

public sealed record BookingDal(
    long Id,
    long UserId,
    long SeatId,
    DateTimeOffset CreatedAt,
    DateOnly BookingDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    BookingStatus Status,
    Decimal Price,
    int SeatNumber,
    int Floor,
    string CoworkingAddress,
    PaymentDal? Payment);
