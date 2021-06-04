using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{
    public enum SIDE
    {
        Long,
        Short,
        Neutral
    }
    public class TradingSystem
    {
        public int Id { get; set; }
        public string Strategy { get; set; }

        public SIDE Side { get; set; }
        public int Quantity { get; set; }
        public DateTime TradeDate { get; set; }
        public double Trade_PL { get; set; }
        public decimal Trade_Price { get; set; }

    }
}
