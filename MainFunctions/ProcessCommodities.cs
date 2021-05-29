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

        public ProcessCommodities(IFinancialDataAPI financialDataAPI, ITechnicalData technicalData)
        {
            financialDataAPI.Type = Type.COMM;
            _financialDataAPI = financialDataAPI;
            _technicalData = technicalData;
        }
        public void Run()
        {
            //FinancialDataAPI financialDataAPI = new FinancialDataAPI(new GenericPropertyAction(), new System.Net.Http.HttpClient());
            //TechnicalData technicalData = new TechnicalData(new GenericPropertyAction());

            //_financialDataAPI.Type = Type.COMM;
            var commodities = _financialDataAPI.GetExchangeSymbolList<Commodity>();
            foreach (var commodity in commodities)
            {
                _financialDataAPI.GetEod<Commodity>(commodity);
                _technicalData.GetTechnicals<Commodity>(commodity);
            }
        }
    }
}
