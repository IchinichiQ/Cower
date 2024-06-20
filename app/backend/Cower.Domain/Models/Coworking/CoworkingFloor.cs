namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingFloor(
    long Id,
    long CoworkingId,
    int Number,
    Image Image,
    IReadOnlyCollection<CoworkingSeat> Seats);