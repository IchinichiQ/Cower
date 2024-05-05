namespace Cower.Service.Models;

public record RegisterUserRequestBL(
    string Email,
    string Password,
    string? Name,
    string? Surname,
    string? Phone);