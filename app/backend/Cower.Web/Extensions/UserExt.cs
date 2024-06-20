using Cower.Domain.Models;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class UserExt
{
    public static UserDto ToUserDTO(this User user)
    {
        return new UserDto(
            Id: user.Id,
            Email: user.Email,
            Name: user.Name,
            Surname: user.Surname,
            Phone: user.Phone,
            Role: user.Role.Name
        );
    }
}