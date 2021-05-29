using System;
using System.Linq;
using TradingApplication___Console.Models;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.Filters;
using TradingApplication___Console.DAL;
using Type = TradingApplication___Console.DAL.Type;
using TradingApplication___Console.Technicals;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TradingApplication___Console.MainFunctions;
using TradingApplication___Console.MainFunctions.Interface;

// DI, SERILOG, SETTINGS

namespace TradingApplication___Console
{
    class Program
    {
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();
        static void Main(string[] args)
        {
            // create a iclass + class to process stocks
            // create a iclass + class to process commodities
            var ProcessStocks = Container.GetService<IProcessStocks>();
            var ProcessCommodities = Container.GetService<IProcessCommodities>();
            //ProcessStocks processStocks = new ProcessStocks();
            //ProcessCommodities processCommodities = new ProcessCommodities();
            
            #region real application

            ProcessStocks.Run();
            ProcessCommodities.Run();

            #endregion

        }



    }


}
