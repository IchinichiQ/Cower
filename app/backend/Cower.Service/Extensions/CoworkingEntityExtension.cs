using Cower.Data.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingEntityExtension
{
    internal static Coworking ToCoworking(this CoworkingEntity entity)
    {
        return new Coworking(
            entity.Id,
            entity.Address,
            entity.Floors,
            entity.WorkingTimes
                .Select(x => x.ToCoworkingWorkingTime())
                .ToArray());
    }
}