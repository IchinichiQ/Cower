using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models;

namespace Cower.Data.Extensions;

internal static class CoworkingFloorEntityExt
{
    public static CoworkingFloorDal ToCoworkingFloorDal(this CoworkingFloorEntity entity)
    {
        return new CoworkingFloorDal(
            entity.Id,
            entity.CoworkingId,
            entity.Number,
            entity.Image.ToImageDal(),
            entity.Seats.Select(x => x.ToSeatDal()).ToArray());
    }
    
    public static CoworkingFloorInfoDal ToCoworkingFloorInfoDal(this CoworkingFloorEntity entity)
    {
        return new CoworkingFloorInfoDal(
            entity.Id,
            entity.CoworkingId,
            entity.Number,
            entity.Image.ToImageDal());
    }
}