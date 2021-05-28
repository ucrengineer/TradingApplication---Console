using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{
    public class EOD
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal adjusted_close { get; set; }
        public decimal adjusted_open
        {
            get { return close != 0 ? open * (adjusted_close / close) : 0; }
            set {; }
        }
        public decimal adjusted_high
        {
            get { return close != 0 ? high * (adjusted_close / close) : 0; }
            set {; }
        }
        public decimal adjusted_low
        {
            get { return close != 0 ? low * (adjusted_close / close) : 0; }
            set {; }
        }
        public double volume { get; set; }
    }
}
