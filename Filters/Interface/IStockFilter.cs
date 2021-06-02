using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Filters.Interface
{
    public interface IStockFilter
    {
        Boolean FilterByEODs(Stock Stock, int MinPrice, int MinVolume);

        Boolean FilterByFundamentals();


        Boolean FilterByRelativeStrength();

        Task<Boolean> FilterByEODsAsync(Stock Stock, int MinPrice, int MinVolume);
    }
}
