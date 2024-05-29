using Cower.Domain.Models.Coworking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class CoworkingExt
{
    public static CoworkingDto ToCoworkingDto(this Coworking coworking)
    {
        return new CoworkingDto
        {
            Id = coworking.Id,
            Address = coworking.Address,
            Floors = coworking.Floors
                .Select(x => x.ToCoworkingFloorDto())
                .ToArray(),
            WorkingTime = coworking.WorkingTime
                .Select(x => x.ToCoworkingWorkingTimeDto())
                .ToArray()
        };
    }
}