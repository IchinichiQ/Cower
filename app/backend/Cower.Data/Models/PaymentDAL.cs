namespace Cower.Data.Models;

public sealed record PaymentDAL(
    long Id,
    long BookingId,
    string Label,
    bool IsCompleted,
    DateTimeOffset ExpireAt);