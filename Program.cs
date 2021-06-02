using Microsoft.Extensions.DependencyInjection;
using TradingApplication___Console.MainFunctions.Interface;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

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


            //await ProcessStocks.RunAsync();
             //await ProcessCommodities.RunAsync();
            ProcessCommodities.Run();
            #endregion

        }







    }


}
