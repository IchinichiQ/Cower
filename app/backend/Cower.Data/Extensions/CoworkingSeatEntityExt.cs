using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class CoworkingSeatEntityExt
{
    public static CoworkingSeatDal ToSeatDal(this CoworkingSeatEntity entity)
    {
        return new CoworkingSeatDal(
            entity.Id,
            entity.FloorId,
            entity.Number,
            entity.Floor.Number,
            entity.Price,
            entity.Description,
            entity.X,
            entity.Y,
            entity.Width,
            entity.Height,
            entity.Angle,
            entity.Image.ToImageDal());
    }
}