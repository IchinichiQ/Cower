namespace Cower.Domain.Models.Booking;

public record Payment(
    long Id,
    long BookingId,
    string Label,
    bool IsCompleted,
    DateTimeOffset ExpireAt);