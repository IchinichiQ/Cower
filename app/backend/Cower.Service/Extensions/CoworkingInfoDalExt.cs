using Cower.Data.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Services;

namespace Cower.Service.Extensions;

internal static class CoworkingInfoDalExtension
{
    internal static CoworkingInfo ToCoworkingInfo(this CoworkingInfoDal dal, IImageLinkGenerator linkGenerator)
    {
        return new CoworkingInfo(
            dal.Id,
            dal.Address,
            dal.Floors
                .Select(x => x.ToCoworkingFloorInfo(linkGenerator))
                .ToArray(),
            dal.WorkingTimes
                .Select(x => x.ToCoworkingWorkingTime())
                .ToArray());
    }
}