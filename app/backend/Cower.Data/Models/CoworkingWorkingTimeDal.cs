namespace Cower.Data.Models;

public record CoworkingWorkingTimeDal(
    long CoworkingId,
    int DayOfWeek,
    TimeOnly Open,
    TimeOnly Close);