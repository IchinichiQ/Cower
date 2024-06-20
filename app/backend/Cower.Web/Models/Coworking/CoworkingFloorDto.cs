namespace Cower.Web.Models;

public sealed class CoworkingFloorDto
{
    public long Id { get; set; }
    public long CoworkingId { get; set; }
    public int Number { get; set; }
    public ImageDto Image { get; set; }
    public IReadOnlyCollection<CoworkingSeatDto> Seats { get; set; }
}