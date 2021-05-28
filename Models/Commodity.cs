using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{
    public class Commodity
    {
        public int id { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        public List<EOD> EODs { get; set; }
        public List<Technical> Technicals { get; set; }

    }
}
