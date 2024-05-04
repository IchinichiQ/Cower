using Microsoft.AspNetCore.Identity;

namespace Cower.Common;

public class ApplicationUser : IdentityUser
{
    public long Id { get; set; }
    public string Email { get; set; }
}