using System.Security.Cryptography;
using System.Text;

namespace Cower.Service.Helpers;

public class PasswordHashingHelper
{
    private static readonly string PASSWORD_SALT;

    static PasswordHashingHelper()
    {
        PASSWORD_SALT = Environment.GetEnvironmentVariable("PASSWORD_SALT")!;
    }
    
    public static byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + PASSWORD_SALT));
        
        return hash;
    }
}