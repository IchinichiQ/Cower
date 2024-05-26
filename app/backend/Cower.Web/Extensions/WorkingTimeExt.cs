using Cower.Domain.Models.Coworking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class WorkingTimeExt
{
    public static CoworkingWorkingTimeDto ToCoworkingWorkingTimeDto(this CoworkingWorkingTime workingTime)
    {
        return new CoworkingWorkingTimeDto
        {
            Day = workingTime.Day.ToString(),
            Open = workingTime.Open.ToString(),
            Close = workingTime.Close.ToString()
        };
    }
}