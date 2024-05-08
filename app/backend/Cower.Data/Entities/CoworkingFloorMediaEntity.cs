using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Entities;

public class CoworkingFloorMediaEntity
{
    public long Id { get; set; }
    public int Number { get; set; }
    public string BackgroundFilename { get; set; } = default!;
    public long CoworkingId { get; set; }
    
    [ForeignKey("CoworkingId")]
    public CoworkingEntity Coworking { get; set; }
}