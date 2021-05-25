using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{
    class Stock
    {
        public int id { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        public List<EOD> EODs { get; set; }
    }
}
