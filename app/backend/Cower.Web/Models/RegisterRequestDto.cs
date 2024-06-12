using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Email не указан")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Пароль не указан")]
    [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
    public string Password { get; init; }

    public string? Name { get; init; }
    
    public string? Surname { get; init; }
    
    [RegularExpression(@"^(8|\+7)[\- ]?(\(?\d{3}\)?[\- ]?)(\d[\- ]?){6}\d$", ErrorMessage = "Некорректный формат номера телефона")]
    public string? Phone { get; init; }
}