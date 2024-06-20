using Cower.Data.Models;
using Cower.Domain.Models.Coworking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class CoworkingFloorInfoExt
{
    public static CoworkingFloorInfoDto ToCoworkingFloorInfoDto(this CoworkingFloorInfo floor)
    {
        return new CoworkingFloorInfoDto
        {
            Id = floor.Id,
            CoworkingId = floor.CoworkingId,
            Number = floor.Number,
            Image = floor.Image.ToImageDto()
        };
    }
}