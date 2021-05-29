using System;
using System.Collections.Generic;
using System.Text;
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

        public ProcessStocks(IFinancialDataAPI financialDataAPI, ITechnicalData technicalData, IStockFilter stockFilter)
        {
            financialDataAPI.Type = Type.US;
            _financialDataAPI = financialDataAPI;
            _technicalData = technicalData;
            _stockFilter = stockFilter;
        }
        public void Run()
        {
            //FinancialDataAPI financialDataAPI = new FinancialDataAPI(new GenericPropertyAction(), new System.Net.Http.HttpClient());

            //StockFilter stockFilter = new StockFilter();
            //TechnicalData technicalData = new TechnicalData(new GenericPropertyAction());
            //_financialDataAPI.Type = Type.US;
            var stocks = _financialDataAPI.GetExchangeSymbolList<Stock>();
            foreach (var stock in stocks)
            {
                _financialDataAPI.GetEod<Stock>(stock);
                if (_stockFilter.FilterByEODs(stock, 3, 20000))
                {
                    _technicalData.GetTechnicals<Stock>(stock);
                }
            }

        }
    }
}
