using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class CoworkingWorkingTimeEntityExt
{
    public static CoworkingWorkingTimeDal ToCoworkingWorkingTimeDal(this CoworkingWorkingTimeEntity entity)
    {
        return new CoworkingWorkingTimeDal(
            entity.CoworkingId,
            entity.DayOfWeek,
            entity.Open,
            entity.Close);
    }
}