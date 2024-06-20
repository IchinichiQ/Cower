using Cower.Domain.Models;

namespace Cower.Data.Models;

public sealed record PasswordResetTokenDal(
    Guid Token,
    User User,
    DateTimeOffset ExpireAt);