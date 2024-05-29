namespace Cower.Data.Models;

public sealed record UpdateCoworkingDal(
    long Id,
    string? Address,
    IReadOnlyCollection<UpdateCoworkingWorkingTimeDal>? WorkingTimes);