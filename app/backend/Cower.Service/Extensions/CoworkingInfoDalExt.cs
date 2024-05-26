using Cower.Data.Models;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingInfoDalExtension
{
    internal static CoworkingInfo ToCoworkingInfo(this CoworkingInfoDal dal)
    {
        return new CoworkingInfo(
            dal.Id,
            dal.Address,
            dal.Floors.Select(x => x.ToCoworkingFloorInfo()).ToArray(),
            dal.WorkingTimes.Select(x => x.ToCoworkingWorkingTime()).ToArray());
    }
}