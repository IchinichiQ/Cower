namespace Cower.Presentation.Models;

public record LoginResponseDTO(
    UserResponseDTO User,
    string Jwt);