using System;
using System.Collections.Generic;
using System.Text;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Technicals.Interface
{
    public interface ITechnicalData
    {
        List<Technical> GetTechnicals<T>(T t);

    }
}
