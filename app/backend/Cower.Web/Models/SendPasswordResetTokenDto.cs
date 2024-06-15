using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class SendPasswordResetTokenDto
{
    [Required(ErrorMessage = "Email не указан")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; }
}