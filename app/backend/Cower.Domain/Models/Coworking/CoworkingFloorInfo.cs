namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingFloorInfo(
    long Id,
    long CoworkingId,
    int Number,
    Image Image);