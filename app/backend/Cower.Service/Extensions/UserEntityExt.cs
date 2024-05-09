using Cower.Data.Models.Entities;
using Cower.Domain.Models;

namespace Cower.Service.Extensions;

internal static class UserEntityExt
{
    internal static User ToUser(this UserEntity userEntity)
    {
        return new User(
            userEntity.Id,
            userEntity.Email,
            userEntity.PasswordHash,
            userEntity.Name,
            userEntity.Surname,
            userEntity.Phone,
            userEntity.Role.ToRole());
    }
}