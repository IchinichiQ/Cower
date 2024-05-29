namespace Cower.Service.Extensions;

internal static class IntExtension
{
    internal static DayOfWeek ToDayOfWeek(this int dayOfWeekNumber)
    {
        return dayOfWeekNumber switch
        {
            0 => DayOfWeek.Monday,
            1 => DayOfWeek.Tuesday,
            2 => DayOfWeek.Wednesday,
            3 => DayOfWeek.Thursday,
            4 => DayOfWeek.Friday,
            5 => DayOfWeek.Saturday,
            6 => DayOfWeek.Sunday,
            _ => throw new ArgumentException("Invalid day of week number. It must be between 0 and 6."),
        };
    }
}
