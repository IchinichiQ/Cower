namespace Cower.Domain.Models.Coworking;

public record CoworkingInfo(
    long Id,
    string Address,
    IReadOnlyCollection<CoworkingFloorInfo> Floors,
    IReadOnlyCollection<CoworkingWorkingTime> WorkingTime);