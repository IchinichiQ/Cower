using Cower.Domain.Models.Statistics;

namespace Cower.Service.Extensions;

public static class StatisticsStepExt
{
    public static int ToDaysCount(this StatisticsStep step)
    {
        return step switch
        {
            StatisticsStep.Day => 1,
            StatisticsStep.Week => 7,
            StatisticsStep.Month => 30,
            StatisticsStep.Year => 365,
            _ => throw new NotImplementedException($"Unknown StatisticsStep type {step.ToString()}")
        };
    }
}