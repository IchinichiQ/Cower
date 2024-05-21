using Cower.Data.Models.Entities;
using Cower.Domain.Models;

namespace Cower.Service.Extensions;

internal static class RoleEntityExt
{
    internal static Role ToRole(this RoleEntity roleEntity)
    {
        return new Role(
            roleEntity.Name,
            roleEntity.Id);
    } 
}