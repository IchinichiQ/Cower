using Cower.Data.Models;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingDalExtension
{
    internal static Coworking ToCoworking(this CoworkingDal entity)
    {
        return new Coworking(
            entity.Id,
            entity.Address,
            entity.Floors.Select(x => x.ToCoworkingFloor()).ToArray(),
            entity.WorkingTimes.Select(x => x.ToCoworkingWorkingTime()).ToArray());
    }
}