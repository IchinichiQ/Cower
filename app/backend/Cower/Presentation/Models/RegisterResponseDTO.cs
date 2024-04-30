namespace Cower.Presentation.Models;

public record RegisterResponseDTO(
    UserResponseDTO User,
    string Jwt);