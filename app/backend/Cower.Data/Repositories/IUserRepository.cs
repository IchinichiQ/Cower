using Cower.Data.Models;

namespace Cower.Data.Repositories;

public interface IUserRepository
{
    public User? GetUser(long id);
    public User? GetUserByCredentials(string email, byte[] password);
    public User AddUser(User user);
}