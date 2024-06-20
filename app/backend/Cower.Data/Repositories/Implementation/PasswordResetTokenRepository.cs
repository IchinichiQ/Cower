using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Repositories.Implementation;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly ApplicationContext _db;

    public PasswordResetTokenRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<PasswordResetTokenDal?> GetToken(Guid token)
    {
        return await _db.PasswordResetTokens
            .Where(x => x.Token == token)
            .Include(x => x.User)
            .ThenInclude(x => x.Role)
            .Select(x => x.ToPasswordResetTokenDal())
            .FirstOrDefaultAsync();
    }

    public async Task<PasswordResetTokenDal> AddToken(long userId, DateTimeOffset expireAt)
    {
        var entity = new PasswordResetTokenEntity
        {
            UserId = userId,
            ExpireAt = expireAt
        };

        await _db.PasswordResetTokens.AddAsync(entity);
        await _db.SaveChangesAsync();

        await _db.Entry(entity).Reference(e => e.User).LoadAsync();
        
        return entity.ToPasswordResetTokenDal();
    }

    public async Task<bool> RemoveToken(Guid token)
    {
        var deleted = await _db.PasswordResetTokens
            .Where(x => x.Token == token)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }
}