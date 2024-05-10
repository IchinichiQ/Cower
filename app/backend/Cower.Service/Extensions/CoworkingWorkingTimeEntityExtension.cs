using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingWorkingTimeEntityExtension
{
    internal static CoworkingWorkingTime ToCoworkingWorkingTime(this CoworkingWorkingTimeEntity entity)
    {
        return new CoworkingWorkingTime(
            entity.DayOfWeek.ToDayOfWeek(),
            entity.Open,
            entity.Close);
    }
}