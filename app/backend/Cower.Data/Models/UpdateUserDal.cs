namespace Cower.Data.Models;

public sealed record UpdateUserDal(
    long Id,
    byte[]? PasswordHash,
    string? Email,
    string? Name,
    string? Surname,
    string? Phone);