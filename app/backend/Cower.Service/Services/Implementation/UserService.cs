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
    private readonly string RESET_PASSWORD_URL;
    
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenService _passwordResetTokenService;
    private readonly IEmailService _emailService;
    
    public UserService(
        ILogger<UserService> logger,
        IUserRepository userRepository,
        IPasswordResetTokenService passwordResetTokenService,
        IEmailService emailService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _passwordResetTokenService = passwordResetTokenService;
        _emailService = emailService;

        PASSWORD_SALT = Environment.GetEnvironmentVariable("PASSWORD_SALT")!;
        RESET_PASSWORD_URL = Environment.GetEnvironmentVariable("RESET_PASSWORD_URL")!;
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

    public async Task<bool> SendPasswordResetToken(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return false;
        }

        var token = await _passwordResetTokenService.AddToken(user.Id);

        var subject = "Восстановление пароля в сервисе Cowёr";
        var message = $"Для восстановления пароля перейдите по ссылке: {RESET_PASSWORD_URL}?token={token.Token}";

        await _emailService.SendEmail(
            user.Email,
            subject,
            message);
        
        return true;
    }

    public async Task ResetPassword(Guid token, string newPassword)
    {
        var resetToken = await _passwordResetTokenService.GetToken(token);
        
        if (resetToken == null)
        {
            throw new PasswordResetTokenDoesntExistException();
        }
        if (resetToken.ExpireAt < DateTime.UtcNow)
        {
            throw new ExpiredPasswordResetTokenException();
        }

        var updateUserDal = new UpdateUserDal(
            resetToken.User.Id,
            HashPassword(newPassword),
            null,
            null,
            null,
            null);

        await _userRepository.UpdateUser(updateUserDal);

        await _passwordResetTokenService.RemoveToken(token);
    }

    private byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + PASSWORD_SALT));
        
        return hash;
    }
}