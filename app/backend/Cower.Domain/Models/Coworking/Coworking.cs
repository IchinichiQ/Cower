namespace Cower.Domain.Models.Coworking;

public record Coworking(
    long Id,
    string Address,
    int Floors,
    IReadOnlyCollection<CoworkingWorkingTime> WorkingTime);