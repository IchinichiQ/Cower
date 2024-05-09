using System.ComponentModel.DataAnnotations;
using Cower.Domain.Models;
using Cower.Service.Exceptions;
using Cower.Service.Models;
using Cower.Service.Services;
using Cower.Web.Helpers;
using Cower.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IJwtService jwtService)
    {
        _logger = logger;
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("api/user/register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register([FromBody] [Required] RegisterRequestDTO request)
    {
        var validationError = ValidationHelper.Validate(request);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var requestBl = new RegisterUserRequestBL(
            request.Email,
            request.Password,
            request.Name,
            request.Surname,
            request.Phone);

        User user;
        try
        {
            user = await _userService.RegisterUser(requestBl);
        }
        catch (EmailTakenException)
        {
            var error = new ErrorDTO(
                ErrorCodes.EMAIL_ALREADY_TAKEN,
                "Пользователь с такой почтой уже существует");
            return BadRequest(error);
        }
        

        var jwt = _jwtService.GenerateJwt(user);

        return new RegisterResponseDTO(
            new UserResponseDTO(
                user.Id,
                user.Email,
                user.Role.Name,
                user.Name,
                user.Surname,
                user.Phone),
            jwt);
    }
    
    [HttpPost("api/user/login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request)
    {
        var validationError = ValidationHelper.Validate(request);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var user = await _userService.TryLogin(request.Email, request.Password);
        if (user == null)
        {
            var error = new ErrorDTO(
                ErrorCodes.INVALID_CREDENTIALS,
                "Пользователя с таким логином и паролем не существует");
            return BadRequest(error);
        }

        var jwt = _jwtService.GenerateJwt(user);

        return new LoginResponseDTO(
            new UserResponseDTO(
                user.Id,
                user.Email,
                user.Role.Name,
                user.Name,
                user.Surname,
                user.Phone),
            jwt);
    }
    
    [HttpGet("api/user/me")]
    [Authorize]
    public async Task<UserInfoResponseDTO> UserInfo()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;
        var user = await _userService.GetUser(long.Parse(userId));

        return new UserInfoResponseDTO(
            new UserResponseDTO(
                user.Id,
                user.Email,
                user.Role.Name,
                user.Name,
                user.Surname,
                user.Phone));
    }
}