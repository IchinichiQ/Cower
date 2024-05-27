using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;
using Cower.Service.Services;

namespace Cower.Service.Extensions;

internal static class CoworkingSeatEntityExtension
{
    internal static CoworkingSeat ToCoworkingSeat(this CoworkingSeatDal dal, IImageLinkGenerator linkGenerator)
    {
        return new CoworkingSeat(
            dal.Id,
            dal.FloorId,
            dal.Number,
            dal.Price,
            dal.Description,
            dal.Image.ToImage(linkGenerator),
            new CoworkingSeatPosition(
                dal.X,
                dal.Y,
                dal.Width,
                dal.Height,
                dal.Angle));
    }
}