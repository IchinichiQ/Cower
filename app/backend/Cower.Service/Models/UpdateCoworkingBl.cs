namespace Cower.Service.Models;

public sealed record UpdateCoworkingBl(
    long Id,
    string? Address,
    IReadOnlyCollection<UpdateCoworkingWorkingTimeBl>? WorkingTimes);