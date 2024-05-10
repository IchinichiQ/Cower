namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingSeat(
    long Id,
    long CoworkingId,
    int Floor,
    int Number,
    decimal Price,
    string ImageFilename,
    string? Description,
    CoworkingSeatPosition Position);