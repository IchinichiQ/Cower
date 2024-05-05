namespace Cower.Web.Models;

public record RegisterResponseDTO(
    UserResponseDTO User,
    string Jwt);