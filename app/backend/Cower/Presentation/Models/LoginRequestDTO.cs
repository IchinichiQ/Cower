using System.ComponentModel.DataAnnotations;

namespace Cower.Presentation.Models;

public class LoginRequestDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}