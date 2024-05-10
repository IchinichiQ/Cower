namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingFloor(
    long Id,
    long CoworkingId,
    int Floor,
    string BackgroundFilename,
    IReadOnlyCollection<CoworkingSeat> Seats);