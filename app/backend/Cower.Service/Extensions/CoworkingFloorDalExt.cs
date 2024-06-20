using Cower.Data.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Services;

namespace Cower.Service.Extensions;

public static class CoworkingFloorDalExt
{
    public static CoworkingFloor ToCoworkingFloor(this CoworkingFloorDal dal, IImageLinkGenerator linkGenerator)
    {
        return new CoworkingFloor(
            dal.Id,
            dal.CoworkingId,
            dal.Number,
            dal.Image.ToImage(linkGenerator),
            dal.Seats.Select(x => x.ToCoworkingSeat(linkGenerator)).ToArray());
    }
}