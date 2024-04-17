namespace Cower.Presentation.Models;

public record UserInfoResponseDTO(
    long Id,
    string Name,
    string Surname,
    string Email,
    string Phone,
    string Role);
