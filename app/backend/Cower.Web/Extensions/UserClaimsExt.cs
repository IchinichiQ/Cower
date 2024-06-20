using System.Security.Claims;

namespace Cower.Web.Extensions;

internal static class UserClaimsExt
{
    public static long GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == "UserId")!.Value;

        return long.Parse(id);
    }

    public static string GetRoleName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)!.Value;
    }
}