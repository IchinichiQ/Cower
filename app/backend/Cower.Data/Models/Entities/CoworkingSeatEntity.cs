using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Models.Entities;

[Index(nameof(FloorId), nameof(Number), IsUnique = true)]
public class CoworkingSeatEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long FloorId { get; set; }
    public decimal Price { get; set; }
    public long ImageId { get; set; } = default!;
    public string? Description { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Number { get; set; }
    
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Angle { get; set; }
    
    [ForeignKey(nameof(FloorId))]
    public CoworkingFloorEntity Floor { get; set; }
    
    [ForeignKey(nameof(ImageId))]
    public ImageEntity Image { get; set; }
}