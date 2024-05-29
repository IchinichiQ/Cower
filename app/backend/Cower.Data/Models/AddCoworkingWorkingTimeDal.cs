namespace Cower.Data.Models;

public sealed record AddCoworkingWorkingTimeDal(
    int DayOfWeek,
    TimeOnly Open,
    TimeOnly Close);