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

[Route("api/v1/users")]
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

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] [Required] RegisterRequestDto request)
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
            var error = new ErrorDto(
                ErrorCodes.EMAIL_ALREADY_TAKEN,
                "Пользователь с такой почтой уже существует");
            return BadRequest(error);
        }
        

        var jwt = _jwtService.GenerateJwt(user);

        return new RegisterResponseDto(
            new UserResponseDto(
                user.Id,
                user.Email,
                user.Role.Name,
                user.Name,
                user.Surname,
                user.Phone),
            jwt);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var validationError = ValidationHelper.Validate(request);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var user = await _userService.TryLogin(request.Email, request.Password);
        if (user == null)
        {
            var error = new ErrorDto(
                ErrorCodes.INVALID_CREDENTIALS,
                "Пользователя с таким логином и паролем не существует");
            return BadRequest(error);
        }

        var jwt = _jwtService.GenerateJwt(user);

        return new LoginResponseDto(
            new UserResponseDto(
                user.Id,
                user.Email,
                user.Role.Name,
                user.Name,
                user.Surname,
                user.Phone),
            jwt);
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<UserInfoResponseDto> UserInfo()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;
        var user = await _userService.GetUser(long.Parse(userId));

        return new UserInfoResponseDto(
            new UserResponseDto(
                user.Id,
                user.Email,
                user.Role.Name,
                user.Name,
                user.Surname,
                user.Phone));
    }
}