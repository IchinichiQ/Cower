namespace Cower.Presentation.Models;

public record LoginResponseDTO(
    long Id,
    string Email,
    string Role,
    string? Name,
    string? Surname,
    string? Phone,
    string Jwt);