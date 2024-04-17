namespace Cower.Presentation.Models;

public record LoginResponseDTO(
    long userId,
    string jwt);