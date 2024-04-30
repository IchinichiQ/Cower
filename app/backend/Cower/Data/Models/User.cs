using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public long Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public long RoleId { get; set; }

    public Role Role { get; set; }
}