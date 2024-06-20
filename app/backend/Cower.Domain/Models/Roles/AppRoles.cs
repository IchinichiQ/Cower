namespace Cower.Domain.Models;

public static class AppRoles
{
    public static readonly Role Admin = new Role(AppRoleNames.Admin, 1);
    public static readonly Role User = new Role(AppRoleNames.User, 2);
}