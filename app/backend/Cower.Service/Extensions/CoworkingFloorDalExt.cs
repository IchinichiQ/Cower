using Cower.Data.Models;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

public static class CoworkingFloorDalExt
{
    public static CoworkingFloor ToCoworkingFloor(this CoworkingFloorDal dal)
    {
        return new CoworkingFloor(
            dal.Id,
            dal.CoworkingId,
            dal.Number,
            dal.Image,
            dal.Seats.Select(x => x.ToCoworkingSeat()).ToArray());
    }
}