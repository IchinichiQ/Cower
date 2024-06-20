namespace Cower.Web.Models;

public record LoginResponseDto(
    UserDto User,
    string Jwt);