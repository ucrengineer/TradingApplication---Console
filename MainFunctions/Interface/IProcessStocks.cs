using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingApplication___Console.MainFunctions.Interface
{
    public interface IProcessStocks
    {
        void Run();

        Task RunAsync();
    }
}
