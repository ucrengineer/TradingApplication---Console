using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TradingApplication___Console.DAL;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.MainFunctions.Interface;
using TradingApplication___Console.Models;
using TradingApplication___Console.Technicals;
using TradingApplication___Console.Technicals.Interface;
using Type = TradingApplication___Console.DAL.Type;

namespace TradingApplication___Console.MainFunctions
{
    public class ProcessCommodities:IProcessCommodities
    {
        private readonly IFinancialDataAPI _financialDataAPI;
        private readonly ITechnicalData _technicalData;
        private readonly ILogger<ProcessCommodities> _log;

        public ProcessCommodities(IFinancialDataAPI financialDataAPI, ITechnicalData technicalData, ILogger<ProcessCommodities> log)
        {
            _financialDataAPI = financialDataAPI;
            _technicalData = technicalData;
            _log = log;
        }
        public void Run()
        {
            _financialDataAPI.Type = Type.COMM;
            var commodities = _financialDataAPI.GetExchangeSymbolList<Commodity>();
            foreach (var commodity in commodities)
            {
                _financialDataAPI.GetEod<Commodity>(commodity);
                _technicalData.GetTechnicals<Commodity>(commodity);
                _log.LogInformation("Commodity {comm} Processed", commodity.Code);
            }
        }
    }
}
