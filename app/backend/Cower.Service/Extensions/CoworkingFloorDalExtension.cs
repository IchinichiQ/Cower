using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Service.Extensions;

internal static class CoworkingFloorDalExtension
{
    internal static CoworkingFloor ToCoworkingFloor(this CoworkingFloorDAL dal)
    {
        return new CoworkingFloor(
            dal.Floor.Id,
            dal.Floor.CoworkingId,
            dal.Floor.Number,
            dal.Floor.BackgroundFilename,
            dal.Seats.Select(x => x.ToCoworkingSeat()).ToArray());
    }
}