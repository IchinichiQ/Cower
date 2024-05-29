namespace Cower.Service.Models;

public record UpdateCoworkingWorkingTimeBl(
    DayOfWeek DayOfWeek,
    TimeOnly Open,
    TimeOnly Close);