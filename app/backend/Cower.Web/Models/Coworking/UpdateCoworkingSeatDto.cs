namespace Cower.Web.Models.Coworking;

public sealed class UpdateCoworkingSeatDto
{
    public long? FloorId { get; set; }
    public int? Number { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public long? ImageId { get; set; }
    public int? X { get; set; }
    public int? Y { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public double? Angle { get; set; }
}