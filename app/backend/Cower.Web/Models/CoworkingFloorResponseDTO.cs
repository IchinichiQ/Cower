namespace Cower.Web.Models;

public sealed class CoworkingFloorResponseDTO
{
    public long CoworkingId { get; set; }
    public int Floor { get; set; }
    public string BackgroundImage { get; set; } = default!;
    public IReadOnlyCollection<CoworkingSeatResponseDTO> Seats { get; set; }
}