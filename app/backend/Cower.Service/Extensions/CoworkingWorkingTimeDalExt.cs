using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingWorkingTimeDalExt
{
    internal static CoworkingWorkingTime ToCoworkingWorkingTime(this CoworkingWorkingTimeDal dal)
    {
        return new CoworkingWorkingTime(
            dal.DayOfWeek.ToDayOfWeek(),
            dal.Open,
            dal.Close);
    }
}