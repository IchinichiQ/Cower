using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Models.Entities;

public class PasswordResetTokenEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Token { get; set; }
    public long UserId { get; set; }
    public DateTimeOffset ExpireAt { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }
}