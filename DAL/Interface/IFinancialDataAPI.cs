using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.DAL.Interface
{
    public interface IFinancialDataAPI
    {
        Type Type { get; set; }
        List<T> GetExchangeSymbolList<T>();

        void GetEod<T>(T t);
    }
}
