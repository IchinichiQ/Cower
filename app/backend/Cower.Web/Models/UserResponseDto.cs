namespace Cower.Web.Models;

public record UserResponseDto(
    long Id,
    string Email,
    string Role,
    string? Name,
    string? Surname,
    string? Phone);