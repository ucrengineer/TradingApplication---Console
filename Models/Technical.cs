using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{
    public class Technical
    {
        public int ID { get; set; }
        public string TICKER { get; set; }
        public string MARKET { get; set; }
        public DateTime DATE { get; set; }
        public float RS_YEAR { get; set; }
        public float RS_HALF_YEAR { get; set; }
        public float RS_QTR_YEAR { get; set; }
        public float P_YEAR { get; set; }
        public float P_HALF_YEAR { get; set; }
        public float P_QTR_YEAR { get; set; }
        public float V_YEAR { get; set; }
        public float V_HALF_YEAR { get; set; }
        public float V_QTR_YEAR { get; set; }
        public float MA_200 { get; set; }
        public float MA_150 { get; set; }
        public float MA_50 { get; set; }
        public float MA_10 { get; set; }


    }
}
