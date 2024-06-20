using Cower.Data.Models.Entities;
using Cower.Domain.Models;

namespace Cower.Data.Models;

public sealed record CoworkingFloorDal(
    long Id,
    long CoworkingId,
    int Number,
    ImageDal Image,
    IReadOnlyCollection<CoworkingSeatDal> Seats);