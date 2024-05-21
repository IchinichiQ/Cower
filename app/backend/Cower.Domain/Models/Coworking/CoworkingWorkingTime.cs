namespace Cower.Domain.Models.Coworking;

public record CoworkingWorkingTime(
    DayOfWeek Day,
    TimeOnly Open,
    TimeOnly Close);