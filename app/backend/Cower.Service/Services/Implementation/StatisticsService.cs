using Cower.Data.Repositories;
using Cower.Domain.Models.Statistics;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class StatisticsService : IStatisticsService
{
    private readonly ILogger<StatisticsService> _logger;
    private readonly IBookingRepository _bookingRepository;

    public StatisticsService(
        ILogger<StatisticsService> logger,
        IBookingRepository bookingRepository)
    {
        _logger = logger;
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingsCountStatistic> GetBookingsCountStatistic(DateOnly startDate, DateOnly endDate, StatisticsStep step)
    {
        if (startDate > endDate)
        {
            throw new BusinessLogicException("Дата начала должна быть меньше даты окончания");
        }
        
        var days = step.ToDaysCount();

        var values = new List<long>();
        var currentStartDate = startDate;
        while (currentStartDate < endDate)
        {
            var currentEndDate = currentStartDate.AddDays(days);
            currentEndDate = currentEndDate > endDate ? endDate : currentEndDate;

            var count = await _bookingRepository.GetSuccessfulBookingsCount(currentStartDate, currentEndDate);
            values.Add(count);
            
            currentStartDate = currentStartDate.AddDays(days);
        }

        return new BookingsCountStatistic(
            startDate,
            endDate,
            step,
            values);
    }

    public async Task<BookingsPriceStatistic> GetBookingsPriceStatistic(DateOnly startDate, DateOnly endDate, StatisticsStep step)
    {
        if (startDate > endDate)
        {
            throw new BusinessLogicException("Дата начала должна быть меньше даты окончания");
        }
        
        var days = step.ToDaysCount();

        var values = new List<decimal>();
        var currentStartDate = startDate;
        while (currentStartDate < endDate)
        {
            var currentEndDate = currentStartDate.AddDays(days);
            currentEndDate = currentEndDate > endDate ? endDate : currentEndDate;

            var count = await _bookingRepository.GetSuccessfulBookingsPrice(currentStartDate, currentEndDate);
            values.Add(count);
            
            currentStartDate = currentStartDate.AddDays(days);
        }

        return new BookingsPriceStatistic(
            startDate,
            endDate,
            step,
            values);
    }
}