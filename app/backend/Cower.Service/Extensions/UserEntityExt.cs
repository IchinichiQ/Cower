using Cower.Data.Entities;
using Cower.Domain.Models;

namespace Cower.Service.Extensions;

public static class UserEntityExt
{
    public static User ToUser(this UserEntity userEntity)
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