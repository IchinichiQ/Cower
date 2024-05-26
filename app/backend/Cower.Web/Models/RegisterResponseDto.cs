namespace Cower.Web.Models;

public record RegisterResponseDto(
    UserResponseDto User,
    string Jwt);