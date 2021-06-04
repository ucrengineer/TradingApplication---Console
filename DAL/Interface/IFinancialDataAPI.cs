using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingApplication___Console.DAL.Interface
{
    public interface IFinancialDataAPI
    {
        Type Type { get; set; }
        Period Period { get; set; }
        List<T> GetExchangeSymbolList<T>();

        void GetEod<T>(T t);

        Task<List<T>> GetExchangeSymbolListAsync<T>();

        Task GetEodAsync<T>(T t);


    }
}
