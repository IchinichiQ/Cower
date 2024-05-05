using System.Text;
using Cower.Data.Entities;
using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.Extensions.Logging;

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

    public User RegisterUser(RegisterUserRequestBL requestBl)
    {
        var user = new UserEntity
        {
            Name = requestBl.Name,
            Surname = requestBl.Surname,
            Email = requestBl.Email,
            PasswordHash = Encoding.UTF8.GetBytes(requestBl.Password),
            Phone = requestBl.Phone,
            RoleId = 2
        };
        
        return _userRepository.AddUser(user).ToUser();
    }

    public User? TryLogin(string email, string password)
    {
        var passwordHash = Encoding.UTF8.GetBytes(password);

        return _userRepository.GetUserByCredentials(email, passwordHash)?.ToUser();
    }

    public User? GetUser(long id)
    {
        return _userRepository.GetUser(id)?.ToUser();
    }
}