using Cower.Data.Entities;
using Cower.Domain.Models;

namespace Cower.Service.Extensions;

public static class RoleEntityExt
{
    public static Role ToRole(this RoleEntity roleEntity)
    {
        return new Role(
            roleEntity.Name,
            roleEntity.Id);
    } 
}