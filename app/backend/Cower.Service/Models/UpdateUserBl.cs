namespace Cower.Service.Models;

public sealed record UpdateUserBl(
    long Id,
    string? Password,
    string? Email,
    string? Name,
    string? Surname,
    string? Phone);