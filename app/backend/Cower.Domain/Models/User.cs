namespace Cower.Domain.Models;

public record User(
    long Id,
    string Email,
    string? Name,
    string? Surname,
    string? Phone,
    Role Role);