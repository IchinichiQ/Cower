using Cower.Domain.Models;

namespace Cower.Service.Services;

public interface IJwtService
{
    public string GenerateJwt(User user);
}