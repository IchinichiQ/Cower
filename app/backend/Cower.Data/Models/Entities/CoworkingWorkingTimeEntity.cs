using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Models.Entities;

public class CoworkingWorkingTimeEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly Open { get; set; }
    public TimeOnly Close { get; set; }
    public long CoworkingId { get; set; }
    
    [ForeignKey("CoworkingId")]
    public CoworkingEntity Coworking { get; set; }
}