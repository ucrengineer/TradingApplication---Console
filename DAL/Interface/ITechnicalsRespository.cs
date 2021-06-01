using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.DAL.Interface
{
    public interface ITechnicalsRespository
    {
        void UpdateTechnicals(Technical technical);

        Task UpdateTechnicalsAsync(Technical technical);
    }
}
