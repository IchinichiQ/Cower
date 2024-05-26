using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingSeatEntityExtension
{
    internal static CoworkingSeat ToCoworkingSeat(this CoworkingSeatDal dal)
    {
        return new CoworkingSeat(
            dal.Id,
            dal.FloorId,
            dal.Number,
            dal.Price,
            dal.ImageFilename,
            dal.Description,
            new CoworkingSeatPosition(
                dal.X,
                dal.Y,
                dal.Width,
                dal.Height,
                dal.Angle));
    }
}