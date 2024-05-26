namespace Cower.Domain.Models.Coworking;

public record Coworking(
    long Id,
    string Address,
    IReadOnlyCollection<CoworkingFloor> Floors,
    IReadOnlyCollection<CoworkingWorkingTime> WorkingTime);