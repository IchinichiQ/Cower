namespace Cower.Data.Models;

public sealed record UpdateCoworkingWorkingTimeDal(
    int DayOfWeek,
    TimeOnly Open,
    TimeOnly Close);