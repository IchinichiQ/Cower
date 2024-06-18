namespace Cower.Web.Models;

public record RegisterResponseDto(
    UserDto User,
    string Jwt);