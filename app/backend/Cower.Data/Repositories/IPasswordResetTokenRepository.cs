using Cower.Data.Models;
using Cower.Domain.Models;

namespace Cower.Data.Repositories;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetTokenDal?> GetToken(Guid token);
    Task<PasswordResetTokenDal> AddToken(long userId, DateTimeOffset expireAt);
    Task<bool> RemoveToken(Guid token);
}