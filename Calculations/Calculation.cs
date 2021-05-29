using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingApplication___Console.Calculations.Interface;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Calculations
{
    public class Calculation:ICalculation
    {
        public float CalculateVolatility(List<EOD> dailyEODs, int period)
        {
            float average; List<float> numerator = new List<float>();
            if (dailyEODs.Count == period + 1 && dailyEODs.Sum(x => x.adjusted_close) != 0)
            {
                average = (float)dailyEODs.Sum(x => x.adjusted_close) / (period + 1);
                foreach (var x in dailyEODs)
                {
                    numerator.Add((float)Math.Pow((float)x.adjusted_close - average, 2));
                }
                var std = (float)Math.Sqrt(numerator.Sum() / period);
                var volatility = (float)Math.Round(std / average, 3);
                return volatility;
            }
            else
            {
                return default(float);
            }
        }
        public float CalculateRelativeStrength(List<EOD> dailyEODs, int period)
        {
            float average; float rs;
            if (dailyEODs.Count == period + 1 && dailyEODs.Sum(x => x.adjusted_close) != 0)
            {
                average = (float)dailyEODs.Sum(x => x.adjusted_close) / (period + 1);
                rs = (float)dailyEODs.LastOrDefault().adjusted_close / average;
                return (float)Math.Round(rs, 3);
            }
            else
            {
                return default(float);
            }
        }
        public float CalculateResults(List<EOD> dailyEODs, int period)
        {
            float results;
            if (dailyEODs.Count == period + 1 && dailyEODs.FirstOrDefault().adjusted_close != 0)
            {
                results = (float)dailyEODs.LastOrDefault().adjusted_close / (float)dailyEODs.FirstOrDefault().adjusted_close;
                return (float)Math.Round(results, 3);
            }
            else
            {
                return default(float);
            }
        }
        public float CalculateMovingAverage(List<EOD> dailyEODs, int period)
        {
            float average;
            if (dailyEODs.Count == period)
            {
                average = (float)dailyEODs.Sum(x => x.adjusted_close) / period;
                return (float)Math.Round(average, 2);
            }
            else
            {
                return default(float);
            }
        }
        // volume 
        public float CalculateRelativeStrengthVolume(List<EOD> dailyEODs, int period)
        {
            float average; float rs;
            if (dailyEODs.Count == period + 1 && dailyEODs.Sum(x => x.volume) != 0)
            {
                average = (float)dailyEODs.Sum(x => x.volume) / (period + 1);
                rs = (float)dailyEODs.LastOrDefault().volume / average;
                return (float)Math.Round(rs, 3);
            }
            else
            {
                return default(float);
            }
        }

    }
}
