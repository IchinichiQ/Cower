using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ApplicationContext _db;

    public UserRepository(
        ILogger<UserRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<UserEntity?> GetUser(long id)
    {
        return await _db.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<UserEntity?> GetUserByCredentials(string email, byte[] password)
    {
        return await _db.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == password);
    }
    
    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _db.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<UserEntity> AddUser(UserEntity user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        await _db.Entry(user)
            .Reference(u => u.Role)
            .LoadAsync();
        
        return user;
    }
    
    public async Task<UserEntity?> UpdateUser(UpdateUserDal dal)
    {
        var updated = await _db.Users
            .Where(x => x.Id == dal.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.PasswordHash, x => dal.PasswordHash ?? x.PasswordHash)
                .SetProperty(x => x.Email, x => dal.Email ?? x.Email)
                .SetProperty(x => x.Name, x => dal.Name ?? x.Name)
                .SetProperty(x => x.Surname, x => dal.Surname ?? x.Surname)
                .SetProperty(x => x.Phone, x => dal.Phone ?? x.Phone));

        if (updated == 0)
        {
            return null;
        }
    
        return await GetUser(dal.Id);
    }
}