namespace Cower.Web.Models;

public sealed class CoworkingSeatResponseDTO
{
    public long Id { get; set; }
    public long CoworkingId { get; set; }
    public int Floor { get; set; }
    public int Number { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; }
    public CoworkingSeatPositionResponseDTO Position { get; set; }
}