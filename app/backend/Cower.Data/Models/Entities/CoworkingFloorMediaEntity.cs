using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Models.Entities;

public class CoworkingFloorMediaEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public int Number { get; set; }
    public string BackgroundFilename { get; set; } = default!;
    public long CoworkingId { get; set; }
    
    [ForeignKey("CoworkingId")]
    public CoworkingEntity Coworking { get; set; }
}