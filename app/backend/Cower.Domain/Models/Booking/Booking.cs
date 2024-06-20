namespace Cower.Domain.Models.Booking;

public record Booking(
    long Id,
    User User,
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
    Payment? Payment,
    bool IsDiscountApplied,
    decimal? InitialPrice);