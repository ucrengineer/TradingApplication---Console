using Microsoft.Extensions.DependencyInjection;
using TradingApplication___Console.MainFunctions.Interface;
using Microsoft.Extensions.Hosting;


namespace TradingApplication___Console
{
    partial class Program
    {
        public static readonly IHost host = new ContainerBuilder().Build();


        static void Main(string[] args)
        {
            
            #region main application
            var ProcessStocks = host.Services.GetService<IProcessStocks>();
            var ProcessCommodities = host.Services.GetService<IProcessCommodities>();


            ProcessStocks.Run();
            //ProcessCommodities.Run();

            #endregion

        }





    }


}
