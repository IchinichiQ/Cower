namespace Cower.Domain.Models.Statistics;

public record BookingsPriceStatistic(
    DateOnly StartDate,
    DateOnly EndDate,
    StatisticsStep Step,
    IReadOnlyCollection<decimal> Values);