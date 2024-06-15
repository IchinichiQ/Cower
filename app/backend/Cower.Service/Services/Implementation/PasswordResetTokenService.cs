using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Service.Extensions;

namespace Cower.Service.Services.Implementation;

public class PasswordResetTokenService : IPasswordResetTokenService
{
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

    private const int TOKEN_EXPIRATION_HOURS = 24;
    
    public PasswordResetTokenService(IPasswordResetTokenRepository passwordResetTokenRepository)
    {
        _passwordResetTokenRepository = passwordResetTokenRepository;
    }

    public async Task<PasswordResetToken> AddToken(long userId)
    {
        var expireAt = DateTimeOffset.UtcNow.AddHours(TOKEN_EXPIRATION_HOURS);
        
        var dal = await _passwordResetTokenRepository.AddToken(userId, expireAt);

        return dal.ToPasswordResetToken();
    }

    public async Task<PasswordResetToken?> GetToken(Guid token)
    {
        var dal = await _passwordResetTokenRepository.GetToken(token);

        return dal?.ToPasswordResetToken();
    }

    public async Task<bool> RemoveToken(Guid token)
    {
        return await _passwordResetTokenRepository.RemoveToken(token);
    }
}