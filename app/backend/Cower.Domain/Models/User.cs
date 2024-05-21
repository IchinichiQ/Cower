namespace Cower.Domain.Models;

public record User(
    long Id,
    string Email,
    byte[] PasswordHash,
    string? Name,
    string? Surname,
    string? Phone,
    Role Role);