using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingApplication___Console.Models
{
    public enum Type
    {
        Stock,
        Commodity
    }
    public class Commodity
    {

        public int id { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        [JsonIgnore()]
        public Type Type { get; set; } = Type.Commodity;
        [JsonProperty("Name")]
        public string Name { get; set; }
        public List<EOD> EODs { get; set; }
        public List<Technical> Technicals { get; set; }

    }
    
}
