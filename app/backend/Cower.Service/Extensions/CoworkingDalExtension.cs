using Cower.Data.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Services;

namespace Cower.Service.Extensions;

internal static class CoworkingDalExtension
{
    internal static Coworking ToCoworking(this CoworkingDal entity, IImageLinkGenerator linkGenerator)
    {
        return new Coworking(
            entity.Id,
            entity.Address,
            entity.Floors
                .Select(x => x.ToCoworkingFloor(linkGenerator))
                .ToArray(),
            entity.WorkingTimes
                .Select(x => x.ToCoworkingWorkingTime())
                .ToArray());
    }
}