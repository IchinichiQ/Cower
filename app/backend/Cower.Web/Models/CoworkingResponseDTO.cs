using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public sealed class CoworkingResponseDTO
{
    [Required]
    public long Id { get; init; }
    [Required]
    public string Address { get; init; }
    [Required]
    public int Floors { get; init; }
    [Required]
    public IReadOnlyCollection<CoworkingWorkingTimeResponseDTO> WorkingTime { get; init; }
}