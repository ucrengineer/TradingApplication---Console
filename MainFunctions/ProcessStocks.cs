using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.DAL;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.Filters;
using TradingApplication___Console.Filters.Interface;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.MainFunctions.Interface;
using TradingApplication___Console.Models;
using TradingApplication___Console.Technicals;
using TradingApplication___Console.Technicals.Interface;
using Type = TradingApplication___Console.DAL.Type;

namespace TradingApplication___Console.MainFunctions
{
    public class ProcessStocks:IProcessStocks
    {

        private readonly IFinancialDataAPI _financialDataAPI;
        private readonly ITechnicalData _technicalData;
        private readonly IStockFilter _stockFilter;
        private readonly ILogger<ProcessStocks> _log;

        public ProcessStocks(IFinancialDataAPI financialDataAPI, ITechnicalData technicalData, IStockFilter stockFilter, ILogger<ProcessStocks> log)
        {
            _financialDataAPI = financialDataAPI;
            _technicalData = technicalData;
            _stockFilter = stockFilter;
            _log = log;
        }
        public void Run()
        {

            _financialDataAPI.Type = Type.US;
            var stocks = _financialDataAPI.GetExchangeSymbolList<Stock>();
            foreach (var stock in stocks)
            {
                _financialDataAPI.GetEod<Stock>(stock);
                if (_stockFilter.FilterByEODs(stock, 1, 20000))
                {
                    _technicalData.GetTechnicals<Stock>(stock);
                    _technicalData.GetTechnicals<Stock>(stock);
                    //_log.LogInformation("Stock {stock} Processed.", stock.Code);
                }
            }
            _log.LogWarning("{number} stocks processed", stocks.Count());
            _log.LogWarning("Application Complete");


        }

        public Task RunAsync()
        {
            return Task.Run(() => Run());
        }
    }
}
