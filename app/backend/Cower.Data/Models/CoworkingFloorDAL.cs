using Cower.Data.Models.Entities;

namespace Cower.Data.Models;

public sealed record CoworkingFloorDAL(
    CoworkingFloorMediaEntity Floor,
    IReadOnlyCollection<CoworkingSeatEntity> Seats);