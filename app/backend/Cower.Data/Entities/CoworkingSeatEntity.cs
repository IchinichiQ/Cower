using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Entities;

public class CoworkingSeatEntity
{
    public long Id { get; set; }
    public int Floor { get; set; }
    public decimal Price { get; set; }
    public string ImageFilename { get; set; } = default!;
    public string? Description { get; set; }
    public long CoworkingId { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Number { get; set; }
    
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Angle { get; set; }
    
    [ForeignKey("CoworkingId")]
    public CoworkingEntity Coworking { get; set; }
}