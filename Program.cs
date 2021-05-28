using System;
using System.Linq;
using TradingApplication___Console.Models;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.Filters;
using TradingApplication___Console.DAL;
using Type = TradingApplication___Console.DAL.Type;
using TradingApplication___Console.Technicals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TradingApplication___Console
{
    class Program
    {

        static void Main(string[] args)
        {


            FinancialDataAPI financialDataAPI = new FinancialDataAPI(new GenericPropertyAction());
            //var test = serviceProvider.GetService<FinancialDataAPI>();

            financialDataAPI.Type = Type.US;
            StockFilter stockFilter = new StockFilter();
            TechnicalData technicalData = new TechnicalData(new GenericPropertyAction());

            var stocks = financialDataAPI.GetExchangeSymbolList<Stock>();
            foreach (var stock in stocks)
            {
                financialDataAPI.GetEod<Stock>(stock);
                if (stockFilter.FilterByEODs(stock, 3, 20000))
                {
                    technicalData.GetTechnicals<Stock>(stock);
                }
            }

            financialDataAPI.Type = Type.COMM;
            var commodities = financialDataAPI.GetExchangeSymbolList<Commodity>();
            foreach(var commodity in commodities)
            {
                financialDataAPI.GetEod<Commodity>(commodity);
                technicalData.GetTechnicals<Commodity>(commodity);
            }


        }

    }
}
