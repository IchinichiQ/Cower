namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingSeat(
    long Id,
    long FloorId,
    int Number,
    decimal Price,
    string ImageFilename,
    string? Description,
    CoworkingSeatPosition Position);