namespace Cower.Web.Models.Coworking;

public class CreateCoworkingSeatDto
{
    public long FloorId { get; set; }
    public int Number { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public long ImageId { get; set; }
    public CoworkingSeatPositionDto Position { get; set; }
}