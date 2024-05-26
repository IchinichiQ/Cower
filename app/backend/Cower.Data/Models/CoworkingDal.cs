namespace Cower.Data.Models;

public record CoworkingDal(
    long Id,
    string Address,
    IReadOnlyCollection<CoworkingFloorDal> Floors,
    IReadOnlyCollection<CoworkingWorkingTimeDal> WorkingTimes);