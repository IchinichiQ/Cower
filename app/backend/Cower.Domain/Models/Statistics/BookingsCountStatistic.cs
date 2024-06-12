namespace Cower.Domain.Models.Statistics;

public record BookingsCountStatistic(
    DateOnly StartDate,
    DateOnly EndDate,
    StatisticsStep Step,
    IReadOnlyCollection<long> Values);