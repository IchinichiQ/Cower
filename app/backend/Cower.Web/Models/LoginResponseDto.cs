namespace Cower.Web.Models;

public record LoginResponseDto(
    UserResponseDto User,
    string Jwt);