using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class CoworkingFloorDalExt
{
    public static CoworkingFloorEntity ToCoworkingFloorEntity(this CoworkingFloorDal dal)
    {
        return new CoworkingFloorEntity
        {
            Id = dal.Id,
            CoworkingId = dal.CoworkingId,
            Number = dal.Number,
            ImageId = dal.Image.Id
        };
    }
}