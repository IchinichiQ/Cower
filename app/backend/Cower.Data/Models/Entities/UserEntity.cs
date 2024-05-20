using System.ComponentModel.DataAnnotations.Schema;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Models.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public long RoleId { get; set; }

    [ForeignKey("RoleId")]
    public RoleEntity Role { get; set; }
    
    [InverseProperty("User")]
    public ICollection<BookingEntity> Bookings { get; set; }
}