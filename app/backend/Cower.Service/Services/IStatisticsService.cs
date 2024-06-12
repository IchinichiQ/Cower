using Cower.Domain.Models.Statistics;

namespace Cower.Service.Services;

public interface IStatisticsService
{
    Task<BookingsCountStatistic> GetBookingsCountStatistic(
        DateOnly startDate,
        DateOnly endDate,
        StatisticsStep step);
    Task<BookingsPriceStatistic> GetBookingsPriceStatistic(
        DateOnly startDate,
        DateOnly endDate,
        StatisticsStep step);
}