using Cower.Data.Models;

namespace Cower.Service;

public interface IJwtService
{
    public string GenerateJwt(User user);
}