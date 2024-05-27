namespace Cower.Web.Models;

public sealed class CoworkingSeatDto
{
    public long Id { get; set; }
    public long FloorId { get; set; }
    public int Number { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public ImageDto Image { get; set; }
    public CoworkingSeatPositionDto Position { get; set; }
}