using System.ComponentModel.DataAnnotations;
using Cower.Web.Attributes;

namespace Cower.Web.Models.Coworking;

public class UpdateCoworkingWorkingTimeDto
{
    [Required]
    [DayOfWeek]
    public string Day { get; set; }
    
    [Required]
    [TimeOnly]
    public string Open { get; set; }
    
    [Required]
    [TimeOnly]
    public string Close { get; set; }
}