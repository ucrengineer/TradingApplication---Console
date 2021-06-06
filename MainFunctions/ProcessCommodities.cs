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
            foreach (var commodity in commodities)
            {
                 _financialDataAPI.GetEod<Commodity>(commodity);
                 _technicalData.GetTechnicalsAsync<Commodity>(commodity);
                //var results = _systems.TheNWeekRuleAsync(commodity).GetAwaiter().GetResult();

                //_log.LogInformation("Commodity {comm} Processed", commodity.Code);
            }

            _log.LogWarning("Application Complete");

            var endTime = DateTime.Now;
            var finishTime = endTime - startTime;
            _log.LogError("Process Time: {time}", finishTime);
            #region simulations
            var Longs = new List<Commodity>(); var shorts = new List<Commodity>();
            foreach(var commodity in commodities)
            {
                //var oldresults = _systems.TheNWeekRuleAsync(commodity).GetAwaiter().GetResult();
                var results = _systems.TheNWeekRuleAndMovingAverageAsync(commodity).GetAwaiter().GetResult();
                foreach(var report in results)
                {
                    //Console.WriteLine($"{commodity.Name}, {report.Side.ToString()}, {report.TradeDate}, {report.Trade_Price}, {report.Trade_PL}");
                }
                //Console.WriteLine($"Average Gain: {results.Where( x => x.Trade_PL > 0).Sum(x => x.Trade_PL) / results.Where(x => x.Trade_PL > 0).Count()}, Average Loss: {results.Where( x => x.Trade_PL < 0).Sum(x => x.Trade_PL) / results.Where(x => x.Trade_PL < 0).Count()}");
                //if (results.Where(x => x.Trade_PL < 0).Count() == 0) { Console.WriteLine("Win/Loss Ratio : No Losses"); }
                //else { Console.WriteLine($"Win/Loss Ratio: {(decimal)results.Where(x => x.Trade_PL > 0).Count() / results.Where(x => x.Trade_PL < 0).Count()}"); };
                //Console.WriteLine($"Regular Weekly System Win/Loss Ratio : {(decimal)oldresults.Where(x => x.Trade_PL > 0).Count() / oldresults.Where(x => x.Trade_PL < 0).Count()}");
                if (results.LastOrDefault().Side == SIDE.Long) { Longs.Add(commodity); };
                if (results.LastOrDefault().Side == SIDE.Short) { shorts.Add(commodity); };
     
            }
            Console.WriteLine("Longs: ");
            foreach(var com in Longs){
                Console.WriteLine($"{com.Code}, {com.Name}");
            }
            Console.WriteLine();
            foreach(var com in shorts)
            {
                Console.WriteLine($"{com.Code}, {com.Name}");
            }
            #endregion
        }

        public Task RunAsync()
        {
                return Task.Run(() => Run());

    
        }
    }
}
