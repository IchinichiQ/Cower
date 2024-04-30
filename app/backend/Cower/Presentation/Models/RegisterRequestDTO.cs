using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cower.Presentation.Models;

public class RegisterRequestDTO
{
    [Required]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }

    public string? Name { get; init; }
    
    public string? Surname { get; init; }
    
    public string? Phone { get; init; }
}
