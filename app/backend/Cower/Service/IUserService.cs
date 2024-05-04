using Cower.Data.Models;
using Cower.Service.Models;

namespace Cower.Service;

public interface IUserService
{
    public User RegisterUser(RegisterUserRequestBL requestBl);
    public User? TryLogin(string email, string password);
    public User? GetUser(long id);
}