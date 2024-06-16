using Cower.Domain.Models;

namespace Cower.Service.Services;

public interface IPasswordResetTokenService
{
    Task<PasswordResetToken> AddToken(long userId);
    Task<PasswordResetToken?> GetToken(Guid token);
    Task<bool> RemoveToken(Guid token);
}