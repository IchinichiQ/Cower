using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

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
