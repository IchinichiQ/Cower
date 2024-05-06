using Cower.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    
    public UserEntity? GetUser(long id)
    {
        using ApplicationContext db = new ApplicationContext();

        return db.Users.Include(x => x.Role).FirstOrDefault(x => x.Id == id);
    }

    public UserEntity? GetUserByCredentials(string email, byte[] password)
    {
        using ApplicationContext db = new ApplicationContext();

        return db.Users.Include(x => x.Role).FirstOrDefault(x => x.Email == email && x.PasswordHash == password);
    }

    public UserEntity AddUser(UserEntity user)
    {
        using ApplicationContext db = new ApplicationContext();

        db.Users.Add(user);
        db.SaveChanges();
        
        db.Entry(user).Reference(u => u.Role).Load();
        
        return user;
    }
}