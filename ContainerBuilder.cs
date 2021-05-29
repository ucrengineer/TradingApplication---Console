using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TradingApplication___Console.Calculations;
using TradingApplication___Console.Calculations.Interface;
using TradingApplication___Console.DAL;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.Filters;
using TradingApplication___Console.Filters.Interface;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.GenericMethods.Interface;
using TradingApplication___Console.MainFunctions;
using TradingApplication___Console.MainFunctions.Interface;
using TradingApplication___Console.Technicals;
using TradingApplication___Console.Technicals.Interface;

namespace TradingApplication___Console
{
    class ContainerBuilder
    {
        public IServiceProvider Build()
        {
            var container = new ServiceCollection();

            // register services
            container.AddSingleton<HttpClient, HttpClient>();
            container.AddSingleton<IFinancialDataAPI, FinancialDataAPI>();
            container.AddSingleton<ICalculation, Calculation>();
            container.AddSingleton<IGenericPropertyAction, GenericPropertyAction>();
            container.AddSingleton<IStockFilter, StockFilter>();
            container.AddSingleton<IProcessStocks, ProcessStocks>();
            container.AddSingleton<IProcessCommodities, ProcessCommodities>();
            container.AddSingleton<ITechnicalData, TechnicalData>();



            return container.BuildServiceProvider();

        }


    }
}
