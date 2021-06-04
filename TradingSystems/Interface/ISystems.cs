using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.TradingSystems.Interface
{
    public interface ISystems
    {
        public Task<List<TradingSystem>> TheNWeekRuleAsync<T>(T t);

        public Task<List<TradingSystem>> TheNWeekRuleAndMovingAverageAsync<T>(T t);
    }
}
