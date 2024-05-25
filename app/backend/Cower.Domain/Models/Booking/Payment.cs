namespace Cower.Domain.Models.Booking;

public record Payment(
    long Id,
    long BookingId,
    string Label,
    string PaymentUrl,
    bool IsCompleted,
    DateTimeOffset ExpireAt);