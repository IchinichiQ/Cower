using Cower.Data.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Services;

namespace Cower.Service.Extensions;

public static class CoworkingFloorInfoDalExt
{
    public static CoworkingFloorInfo ToCoworkingFloorInfo(this CoworkingFloorInfoDal dal, IImageLinkGenerator linkGenerator)
    {
        return new CoworkingFloorInfo(
            dal.Id,
            dal.CoworkingId,
            dal.Number,
            dal.Image.ToImage(linkGenerator));
    }
}