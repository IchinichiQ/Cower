using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Cower.Service.Services.Implementation;

public class UserService : IUserService
{
    private readonly string PASSWORD_SALT;
    
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;
    
    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        
        PASSWORD_SALT = Environment.GetEnvironmentVariable("PASSWORD_SALT")!;
    }

    public async Task<User> RegisterUser(RegisterUserRequestBL requestBl)
    {
        var userEntity = new UserEntity
        {
            Name = requestBl.Name,
            Surname = requestBl.Surname,
            Email = requestBl.Email,
            PasswordHash = HashPassword(requestBl.Password),
            Phone = requestBl.Phone,
            RoleId = AppRoles.User.Id
        };

        try
        {
            await _userRepository.AddUser(userEntity);
            return userEntity.ToUser();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new EmailTakenException();
        }
    }

    public async Task<User?> TryLogin(string email, string password)
    {
        var passwordHash = HashPassword(password);

        var userEntity = await _userRepository.GetUserByCredentials(email, passwordHash);
        
        return userEntity?.ToUser();
    }

    public async Task<User?> GetUser(long id)
    {
        var userEntity = await _userRepository.GetUser(id);
        
        return userEntity?.ToUser();
    }

    public async Task<User?> UpdateUser(UpdateUserBl bl)
    {
        var dal = new UpdateUserDal(
            bl.Id,
            bl.Password == null ? null : HashPassword(bl.Password),
            bl.Email,
            bl.Name,
            bl.Surname,
            bl.Phone);

        var userEntity = await _userRepository.UpdateUser(dal);
        
        return userEntity?.ToUser();
    }

    private byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + PASSWORD_SALT));
        
        return hash;
    }
}