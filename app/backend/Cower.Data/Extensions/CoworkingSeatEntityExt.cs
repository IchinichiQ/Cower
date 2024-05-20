using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class CoworkingSeatEntityExt
{
    public static SeatDAL ToDal(this CoworkingSeatEntity entity)
    {
        return new SeatDAL(
            entity.Id,
            entity.CoworkingId,
            entity.Floor,
            entity.Price,
            entity.ImageFilename,
            entity.Description,
            entity.Number,
            entity.Coworking);
    }
}