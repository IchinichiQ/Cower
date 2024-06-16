using Cower.Data.Models.Entities;
using Cower.Domain.Models;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface IUserService
{
    public Task<User> RegisterUser(RegisterUserRequestBL requestBl);
    public Task<User?> TryLogin(string email, string password);
    public Task<User?> GetUser(long id);
    public Task<User?> UpdateUser(UpdateUserBl bl);
    public Task<bool> SendPasswordResetToken(string email);
    public Task ResetPassword(Guid token, string newPassword);
}