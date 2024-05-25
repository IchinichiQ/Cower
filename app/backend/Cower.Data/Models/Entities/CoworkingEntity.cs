using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Models.Entities;

public class CoworkingEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Address { get; set; }
    public int Floors { get; set; }

    [InverseProperty("Coworking")]
    public ICollection<CoworkingWorkingTimeEntity> WorkingTimes { get; set; }
}