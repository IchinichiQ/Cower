namespace Cower.Domain.Models;

public record PasswordResetToken(
    Guid Token,
    User User,
    DateTimeOffset ExpireAt);