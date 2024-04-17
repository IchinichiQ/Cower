namespace Cower.Presentation.Models;

public record UserInfoResponseDTO(
    long Id,
    string Email,
    string Role,
    string? Name,
    string? Surname,
    string? Phone);
