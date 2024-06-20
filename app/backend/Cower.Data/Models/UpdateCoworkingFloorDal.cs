namespace Cower.Data.Models;

public sealed record UpdateCoworkingFloorDal(
    long Id,
    long? CoworkingId,
    long? ImageId,
    int? Number);