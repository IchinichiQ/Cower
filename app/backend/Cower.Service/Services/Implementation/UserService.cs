using System.Data.SqlClient;
using System.Text;
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
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<User> RegisterUser(RegisterUserRequestBL requestBl)
    {
        var userEntity = new UserEntity
        {
            Name = requestBl.Name,
            Surname = requestBl.Surname,
            Email = requestBl.Email,
            PasswordHash = Encoding.UTF8.GetBytes(requestBl.Password),
            Phone = requestBl.Phone,
            RoleId = AppRoles.User.Id
        };

        try
        {
            await _userRepository.AddUser(userEntity);
            return userEntity.ToUser();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new EmailTakenException();
        }
    }

    public async Task<User?> TryLogin(string email, string password)
    {
        var passwordHash = Encoding.UTF8.GetBytes(password);

        var userEntity = await _userRepository.GetUserByCredentials(email, passwordHash);
        
        return userEntity?.ToUser();
    }

    public async Task<User?> GetUser(long id)
    {
        var userEntity = await _userRepository.GetUser(id);
        
        return userEntity?.ToUser();
    }
}