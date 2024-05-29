namespace Cower.Service.Models;

public record CreateCoworkingWorkingTimeBl(
    DayOfWeek DayOfWeek,
    TimeOnly Open,
    TimeOnly Close);