using System.ComponentModel.DataAnnotations;
using Cower.Domain.Models;

namespace Cower.Web.Models;

public class UploadImageDto
{
    [Required]
    public IFormFile Image { get; set; }
    
    [Required]
    public ImageType Type { get; set; }
}