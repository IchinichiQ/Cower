using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class PasswordResetDto
{
    [Required(ErrorMessage = "Токен восстановления пароля не указан")]
    public Guid PasswordResetToken { get; set; }
    
    [Required(ErrorMessage = "Пароль не указан")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
    public string NewPassword { get; set; }
}