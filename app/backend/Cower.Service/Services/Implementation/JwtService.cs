using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cower.Data.Entities;
using Cower.Domain.JWT;
using Cower.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Cower.Service.Services.Implementation;

public class JwtService : IJwtService
{
    private readonly ILogger<JwtService> _logger;

    public JwtService(ILogger<JwtService> logger)
    {
        _logger = logger;
    }

    public string GenerateJwt(User user)
    {
        var identity = GetIdentity(user);
        
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
    
    private ClaimsIdentity GetIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new ("UserId", user.Id.ToString()),
            new (ClaimsIdentity.DefaultNameClaimType, user.Email),
            new (ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
        };
        
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        
        return claimsIdentity;
    }
}