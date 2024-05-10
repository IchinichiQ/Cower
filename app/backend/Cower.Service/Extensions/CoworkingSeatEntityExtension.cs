using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingSeatEntityExtension
{
    internal static CoworkingSeat ToCoworkingSeat(this CoworkingSeatEntity entity)
    {
        return new CoworkingSeat(
            entity.Id,
            entity.CoworkingId,
            entity.Floor,
            entity.Number,
            entity.Price,
            entity.ImageFilename,
            entity.Description,
            new CoworkingSeatPosition(
                entity.X,
                entity.Y,
                entity.Width,
                entity.Height,
                entity.Angle));
    }
}