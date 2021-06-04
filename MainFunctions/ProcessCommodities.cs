using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.DAL;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.MainFunctions.Interface;
using TradingApplication___Console.Models;
using TradingApplication___Console.Technicals;
using TradingApplication___Console.Technicals.Interface;
using TradingApplication___Console.TradingSystems.Interface;
using Type = TradingApplication___Console.DAL.Type;


namespace TradingApplication___Console.MainFunctions
{
    public class ProcessCommodities:IProcessCommodities
    {
        private readonly IFinancialDataAPI _financialDataAPI;
        private readonly ITechnicalData _technicalData;
        private readonly ILogger<ProcessCommodities> _log;
        private readonly ITechnicalsRespository _technicalsRespository;
        private readonly ISystems _systems;

        public ProcessCommodities(IFinancialDataAPI financialDataAPI, ITechnicalData technicalData, ILogger<ProcessCommodities> log, ITechnicalsRespository technicalsRespository, ISystems systems)
        {
            _financialDataAPI = financialDataAPI;
            _technicalData = technicalData;
            _log = log;
            _technicalsRespository = technicalsRespository;
            _systems = systems;
        }
        public void Run()
        {
            var startTime = DateTime.Now;

            _financialDataAPI.Type = Type.COMM;
            var commodities =  _financialDataAPI.GetExchangeSymbolList<Commodity>();
            var systems = new Dictionary<string,List<TradingSystem>>();
            foreach (var commodity in commodities)
            {
                 _financialDataAPI.GetEod<Commodity>(commodity);
                 _technicalData.GetTechnicalsAsync<Commodity>(commodity);
                //_log.LogInformation("Commodity {comm} Processed", commodity.Code);

            }
            _log.LogWarning("Application Complete");

            var endTime = DateTime.Now;
            var finishTime = endTime - startTime;
            _log.LogError("Process Time: {time}", finishTime);
            _log.LogError("Simulating Systems");
            #region simulations
            foreach (var commodity in commodities)
            {
                try
                {
                    // system testing
                    var results = _systems.TheNWeekRuleAndMovingAverageAsync(commodity).GetAwaiter().GetResult();
                    systems.Add(commodity.Code.ToString(), results);
                }
                catch (Exception ex)
                {
                    _log.LogError("{comm} error : {message}", commodity.Code, ex.Message);
                }

            }
            foreach (var system in systems)
            {
                _log.LogError("{name} - Last Trade: {day} -  Overall Results: {results}", system.Key.ToString(), system.Value.LastOrDefault().TradeDate, system.Value.Sum(x => x.Trade_PL));



            }

            #endregion
        }

        public Task RunAsync()
        {
                return Task.Run(() => Run());

    
        }
    }
}
