﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingApplication___Console.Filters.Interface;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Filters
{
    public class StockFilter:IStockFilter
    {
        private readonly ILogger<StockFilter> _log;

        public StockFilter(ILogger<StockFilter> log)
        {
            _log = log;
        }
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
                _log.LogInformation(e.Message);
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
