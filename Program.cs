using System;
using System.Linq;
using TradingApplication___Console.Models;
using TradingApplication___Console.GenericMethods;
namespace TradingApplication___Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // create objects FinancialDataAPI + Technicals + Filter
            FinancialDataAPI financialDataAPI = new FinancialDataAPI(new GenericPropertyAction());
            financialDataAPI.Type = Type.US;

            var stocks = financialDataAPI.GetExchangeSymbolList<Stock>();
            foreach(var stock in stocks)
            {


                financialDataAPI.GetEod<Stock>(stock);
                //Console.WriteLine(test.GetType());
                // getEOD
                // filter EOD, throw away if boolean = false
                // calculate technicals
                // update database (db class)
               Console.WriteLine(stock.EODs.FirstOrDefault().close);
            }

        }
    }
}
