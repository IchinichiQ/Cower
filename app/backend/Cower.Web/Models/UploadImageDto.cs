using System.ComponentModel.DataAnnotations;
using Cower.Domain.Models;
using Cower.Web.Attributes;

namespace Cower.Web.Models;

public class UploadImageDto
{
    [Required(ErrorMessage = "Отсутствует изображение")]
    public IFormFile Image { get; set; }
    
    [RequiredEnum(ErrorMessage = "Тип изображения не указан")]
    public ImageType Type { get; set; }
}