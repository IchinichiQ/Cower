using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Models.Entities;

public class RoleEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; }
}