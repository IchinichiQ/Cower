using Cower.Data.Models;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

public static class CoworkingFloorInfoDalExt
{
    public static CoworkingFloorInfo ToCoworkingFloorInfo(this CoworkingFloorInfoDal dal)
    {
        return new CoworkingFloorInfo(
            dal.Id,
            dal.CoworkingId,
            dal.Number,
            dal.Image);
    }
}