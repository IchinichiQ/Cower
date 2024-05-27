using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Models.Entities;

[Index(nameof(CoworkingId), nameof(Number), IsUnique = true)]
public class CoworkingFloorEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long CoworkingId { get; set; }
    public int Number { get; set; }
    public long ImageId { get; set; }

    [InverseProperty(nameof(CoworkingSeatEntity.Floor))]
    public ICollection<CoworkingSeatEntity> Seats { get; set; } = new List<CoworkingSeatEntity>();
    
    [ForeignKey(nameof(CoworkingId))]
    public CoworkingEntity Coworking { get; set; }
    
    [ForeignKey(nameof(ImageId))]
    public ImageEntity Image { get; set; }
}