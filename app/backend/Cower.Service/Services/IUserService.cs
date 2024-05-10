using Cower.Data.Entities;
using Cower.Domain.Models;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface IUserService
{
    public Task<User> RegisterUser(RegisterUserRequestBL requestBl);
    public Task<User?> TryLogin(string email, string password);
    public Task<User?> GetUser(long id);
}