using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class CoworkingEntityExt
{
    public static CoworkingDal ToCoworkingDal(this CoworkingEntity entity)
    {
        return new CoworkingDal(
            entity.Id,
            entity.Address,
            entity.Floors.Select(x => x.ToCoworkingFloorDal()).ToArray(),
            entity.WorkingTimes.Select(x => x.ToCoworkingWorkingTimeDal()).ToArray());
    }
    
    public static CoworkingInfoDal ToCoworkingInfoDal(this CoworkingEntity entity)
    {
        return new CoworkingInfoDal(
            entity.Id,
            entity.Address,
            entity.Floors.Select(x => x.ToCoworkingFloorInfoDal()).ToArray(),
            entity.WorkingTimes.Select(x => x.ToCoworkingWorkingTimeDal()).ToArray());
    }
}