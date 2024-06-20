using Cower.Domain.Models.Statistics;

namespace Cower.Web.Models.Statistics;

public class BookingsPriceStatisticsResponseDto
{
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public StatisticsStep Step { get; set; }
    public IReadOnlyCollection<decimal> Values { get; set; }
}
