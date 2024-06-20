using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models.Coworking;

public class CreateCoworkingFloorDto
{
    [Required(ErrorMessage = "ID коворкинга не указан")]
    public long CoworkingId { get; set; }
    
    [Required(ErrorMessage = "ID изображения не указан")]
    public long ImageId { get; set; }
    
    [Required(ErrorMessage = "Номер этажа не указан")]
    public int Number { get; set; }
}