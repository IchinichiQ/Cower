namespace Cower.Service.Models;

public sealed record UpdateFloorBl(
    long Id,
    long? CoworkingId,
    long? ImageId,
    int? Number);