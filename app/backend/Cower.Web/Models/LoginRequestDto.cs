using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email не указан")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Пароль не указан")]
    public string Password { get; init; }
}