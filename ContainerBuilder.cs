using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using TradingApplication___Console.Calculations;
using TradingApplication___Console.Calculations.Interface;
using TradingApplication___Console.DAL;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.Filters;
using TradingApplication___Console.Filters.Interface;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.GenericMethods.Interface;
using TradingApplication___Console.MainFunctions;
using TradingApplication___Console.MainFunctions.Interface;
using TradingApplication___Console.Technicals;
using TradingApplication___Console.Technicals.Interface;

namespace TradingApplication___Console
{
    public class ContainerBuilder
    {
        
  
        public IHost Build()
        {


            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
            // place to put all dependencies
            .ConfigureServices((context, services) =>
            {
                            // put services in this container
                services.AddSingleton<HttpClient, HttpClient>();
                services.AddSingleton<IFinancialDataAPI, FinancialDataAPI>();
                services.AddSingleton<ICalculation, Calculation>();
                services.AddSingleton<IGenericPropertyAction, GenericPropertyAction>();
                services.AddSingleton<IStockFilter, StockFilter>();
                services.AddSingleton<IProcessStocks, ProcessStocks>();
                services.AddSingleton<IProcessCommodities, ProcessCommodities>();
                services.AddSingleton<ITechnicalData, TechnicalData>();
                // Database Connection
                services.AddTransient<IDbConnection>(
                    db => new OracleConnection(builder.Build().GetSection("DataConnections").GetSection("ConnectionString").Value)
                    );

                // training class

                services.AddTransient<IGreetingService, GreetingService>();
            })
            .UseSerilog()
            .Build();
            return host;

        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        
          

   


    }
}
