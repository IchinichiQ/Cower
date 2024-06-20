namespace Cower.Data.Models;

public sealed record PaymentDal(
    long Id,
    long BookingId,
    string Label,
    string PaymentUrl,
    bool IsCompleted,
    DateTimeOffset ExpireAt);