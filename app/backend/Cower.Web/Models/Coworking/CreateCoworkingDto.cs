using System.ComponentModel.DataAnnotations;
using Cower.Service.Models;

namespace Cower.Web.Models.Coworking;

public class CreateCoworkingDto
{
    [Required]
    public string Address { get; set; }
    
    [Required]
    public IReadOnlyCollection<CreateCoworkingWorkingTimeDto> WorkingTimes { get; set; }
}