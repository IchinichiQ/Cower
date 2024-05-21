namespace Cower.Service.Extensions;

internal static class IntExtension
{
    internal static DayOfWeek ToDayOfWeek(this int dayOfWeekNumber)
    {
        return dayOfWeekNumber switch
        {
            1 => DayOfWeek.Monday,
            2 => DayOfWeek.Tuesday,
            3 => DayOfWeek.Wednesday,
            4 => DayOfWeek.Thursday,
            5 => DayOfWeek.Friday,
            6 => DayOfWeek.Saturday,
            7 => DayOfWeek.Sunday,
            _ => throw new ArgumentException("Invalid day of week number. It should be between 1 and 7."),
        };
    }
}
