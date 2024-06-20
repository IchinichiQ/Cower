using Cower.Domain.Models.Coworking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class CoworkingFloorExt
{
    public static CoworkingFloorDto ToCoworkingFloorDto(this CoworkingFloor floor)
    {
        return new CoworkingFloorDto
        {
            Id = floor.Id,
            CoworkingId = floor.CoworkingId,
            Number = floor.Number,
            Image = floor.Image.ToImageDto(),
            Seats = floor.Seats.Select(x => x.ToCoworkingSeatDto()).ToArray()
        };
    }
}