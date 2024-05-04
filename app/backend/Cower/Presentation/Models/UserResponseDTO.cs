namespace Cower.Presentation.Models;

public record UserResponseDTO(
    long Id,
    string Email,
    string Role,
    string? Name,
    string? Surname,
    string? Phone);