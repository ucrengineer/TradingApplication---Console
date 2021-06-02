using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.Technicals.Interface
{
    public interface ITechnicalData
    {
        void GetTechnicals<T>(T t);

        Task GetTechnicalsAsync<T>(T t);

    }
}
