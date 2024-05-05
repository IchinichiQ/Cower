namespace Cower.Web.Models;

public record LoginResponseDTO(
    UserResponseDTO User,
    string Jwt);