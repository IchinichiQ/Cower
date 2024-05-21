using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public sealed class CoworkingWorkingTimeResponseDTO
{
    [Required]
    public string Day { get; init; }
    [Required]
    public string Open { get; init; }
    [Required]
    public string Close { get; init; }
}