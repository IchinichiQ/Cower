namespace Cower.Web.Models;

public sealed class CoworkingFloorInfoDto
{
    public long Id { get; set; }
    public long CoworkingId { get; set; }
    public int Number { get; set; }
    public ImageDto Image { get; set; }
}