using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class UpdateUserDto
{
    [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов")] //TODO: Check
    public string? Password { get; set; }
    
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string? Email { get; set; }
    
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    
    [RegularExpression(@"^(8|\+7)[\- ]?(\(?\d{3}\)?[\- ]?)(\d[\- ]?){6}\d$", ErrorMessage = "Некорректный формат номера телефона")]
    public string? Phone { get; set; }
}