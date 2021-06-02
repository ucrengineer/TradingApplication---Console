using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Calculations.Interface
{
    public interface ICalculation
    {
        float CalculateVolatility(List<EOD> dailyEODs, int period);

        float CalculateRelativeStrength(List<EOD> dailyEODs, int period);
        float CalculateResults(List<EOD> dailyEODs, int period);
        float CalculateMovingAverage(List<EOD> dailyEODs, int period);
        float CalculateRelativeStrengthVolume(List<EOD> dailyEODs, int period);

        Task<float> CalculateVolatilityAsync(List<EOD> dailyEODs, int period);
        Task<float> CalculateRelativeStrengthAsync(List<EOD> dailyEODs, int period);
        Task<float> CalculateResultsAsync(List<EOD> dailyEODs, int period);
        Task<float> CalculateMovingAverageAsync(List<EOD> dailyEODs, int period);


    }
}
