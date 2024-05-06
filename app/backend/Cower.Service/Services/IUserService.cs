using Cower.Data.Entities;
using Cower.Domain.Models;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface IUserService
{
    public User RegisterUser(RegisterUserRequestBL requestBl);
    public User? TryLogin(string email, string password);
    public User? GetUser(long id);
}