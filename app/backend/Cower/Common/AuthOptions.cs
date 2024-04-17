using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Cower.Common;

public class AuthOptions
{
    public const string ISSUER = "Cower";
    public const string AUDIENCE = "CowerUser";
    public static readonly string KEY;
    public const int LIFETIME = 1440;

    static AuthOptions()
    {
        KEY = Environment.GetEnvironmentVariable("JWT_KEY")!;
    }

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}