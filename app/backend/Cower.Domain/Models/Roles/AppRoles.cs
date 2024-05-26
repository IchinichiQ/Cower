namespace Cower.Domain.Models;

public static class AppRoles
{
    public static Role Admin = new Role(AppRoleNames.Admin, 1);
    public static Role User = new Role(AppRoleNames.User, 2);
}