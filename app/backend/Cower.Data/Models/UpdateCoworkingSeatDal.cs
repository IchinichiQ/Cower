using Cower.Domain.Models;
using Cower.Domain.Models.Coworking;

namespace Cower.Data.Models;

public sealed record UpdateCoworkingSeatDal(
    long Id,
    long? FloorId,
    int? Number,
    decimal? Price,
    string? Description,
    long? ImageId,
    int? X,
    int? Y,
    int? Width,
    int? Height,
    double? Angle);