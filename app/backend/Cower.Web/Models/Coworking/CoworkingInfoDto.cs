using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class CoworkingInfoDto
{
    [Required]
    public long Id { get; init; }
    
    [Required]
    public string Address { get; init; }
    
    [Required]
    public IReadOnlyCollection<CoworkingFloorInfoDto> Floors { get; set; }
    
    [Required]
    public IReadOnlyCollection<CoworkingWorkingTimeDto> WorkingTime { get; init; }
}