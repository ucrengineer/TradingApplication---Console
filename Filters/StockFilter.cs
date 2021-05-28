using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Filters
{
    public class StockFilter
    {

        public Boolean FilterByEODs(Stock Stock, int MinPrice, int MinVolume)
        {
            try 
            {
                if (!Stock.EODs.Any() || Stock.EODs.LastOrDefault().adjusted_close < MinPrice
                    || Stock.EODs.LastOrDefault().volume < MinVolume)
                {
                    return false;
                }
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        
        public Boolean FilterByFundamentals()
        {
            return false;
        }
        
        public Boolean FilterByRelativeStrength()
        {
            return false;
        }

    }
}
