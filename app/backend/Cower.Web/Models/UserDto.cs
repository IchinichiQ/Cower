namespace Cower.Web.Models;

public record UserDto(
    long Id,
    string Email,
    string Role,
    string? Name,
    string? Surname,
    string? Phone);