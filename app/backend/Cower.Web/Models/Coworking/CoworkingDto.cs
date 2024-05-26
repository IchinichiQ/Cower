using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public sealed class CoworkingDto
{
    [Required]
    public long Id { get; init; }
    
    [Required]
    public string Address { get; init; }
    
    [Required]
    public IReadOnlyCollection<CoworkingFloorDto> Floors { get; set; }
    
    [Required]
    public IReadOnlyCollection<CoworkingWorkingTimeDto> WorkingTime { get; init; }
}