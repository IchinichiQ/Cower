using Cower.Domain.Models.Coworking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class CoworkingSeatExt
{
    public static CoworkingSeatDto ToCoworkingSeatDto(this CoworkingSeat seat)
    {
        return new CoworkingSeatDto
        {
            Id = seat.Id,
            FloorId = seat.FloorId,
            Number = seat.Number,
            Price = seat.Price,
            Description = seat.Description,
            Image = seat.ImageFilename,
            Position = new CoworkingSeatPositionDto
            {
                X = seat.Position.X,
                Y = seat.Position.Y,
                Width = seat.Position.Width,
                Height = seat.Position.Height,
                Angle = seat.Position.Angle
            }
        };
    }
}