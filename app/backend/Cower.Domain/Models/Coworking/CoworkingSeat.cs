namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingSeat(
    long Id,
    long FloorId,
    int Number,
    decimal Price,
    string? Description,
    Image Image,
    CoworkingSeatPosition Position);