using System;
using TradingApplication___Console.Models;

namespace TradingApplication___Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // create objects FinancialDataAPI + Technicals + Filter
            FinancialDataAPI financialDataAPI = new FinancialDataAPI();
            financialDataAPI.Type = Type.US;

            var stocks = financialDataAPI.GetExchangeSymbolList<Stock>();
            foreach(var stock in stocks)
            {
                Console.WriteLine(stock.Code);
            }
            financialDataAPI.Type = Type.COMM;
            var comms = financialDataAPI.GetExchangeSymbolList<Commodity>();
            foreach(var comm in comms)
            {
                Console.WriteLine(comm.Code);
            }
        }
    }
}
