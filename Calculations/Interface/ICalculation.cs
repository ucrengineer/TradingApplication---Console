using System;
using System.Collections.Generic;
using System.Text;
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


    }
}
