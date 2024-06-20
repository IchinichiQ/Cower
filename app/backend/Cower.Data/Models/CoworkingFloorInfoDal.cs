using Cower.Domain.Models;

namespace Cower.Data.Models;

public sealed record CoworkingFloorInfoDal(
    long Id,
    long CoworkingId,
    int Number,
    ImageDal Image);