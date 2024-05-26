namespace Cower.Data.Models;

public sealed record CoworkingInfoDal(
    long Id,
    string Address,
    IReadOnlyCollection<CoworkingFloorInfoDal> Floors,
    IReadOnlyCollection<CoworkingWorkingTimeDal> WorkingTimes);