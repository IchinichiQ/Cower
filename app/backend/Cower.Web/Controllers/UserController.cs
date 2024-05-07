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

    [HttpPost("/api/user/register")]
    public RegisterResponseDTO Register([FromBody] RegisterRequestDTO request)
    {
        var requestBl = new RegisterUserRequestBL(
            request.Email,
            request.Password,
            request.Name,
            request.Surname,
            request.Phone);

        var user = _userService.RegisterUser(requestBl);

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
    
    [HttpPost("/api/user/login")]
    public ActionResult<LoginResponseDTO> Login([FromBody] LoginRequestDTO request)
    {
        var validationError = ValidationHelper.Validate(request);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }
        
        var user = _userService.TryLogin(request.Email, request.Password);
        if (user == null)
        {
            var error = new ErrorDTO(
                "invalid_credentials",
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
    
    [HttpGet("/api/user/me")]
    [Authorize]
    public UserInfoResponseDTO UserInfo()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;
        var user = _userService.GetUser(long.Parse(userId));

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