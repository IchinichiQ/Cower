using Cower.Data.Models.Entities;
using Cower.Domain.Models;

namespace Cower.Data.Extensions;

internal static class UserEntityExt
{
    public static User ToUser(this UserEntity entity)
    {
        return new User(
            entity.Id,
            entity.Email,
            entity.Name,
            entity.Surname,
            entity.Phone,
            entity.Role.ToRole());
    }   
    
    public static Role ToRole(this RoleEntity entity)
    {
        return new Role(
            entity.Name,
            entity.Id);
    }   
}