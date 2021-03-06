using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{

   public class Stock
    {
        public int id { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        [JsonIgnore()]
        public Type Type { get; set; } = Type.Stock;
        public List<EOD> EODs { get; set; }
        public List<Technical> Technicals { get; set; }
    }
}
