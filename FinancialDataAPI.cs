using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console
{
    public enum Type
    {
        US,
        COMM
    }
 
    public class FinancialDataAPI : HttpClient
    {
        private new Uri BaseAddress { get; set; } = new Uri("https://eodhistoricaldata.com/api/");
        private string APIToken { get; set; } = "api_token=5f51fc52bc5dd8.26338537&fmt=json";

        public Type Type { get; set; }
        public List<T> GetExchangeSymbolList<T>()
        {
           
            var uri = new Uri(this.BaseAddress, $"exchange-symbol-list/{Type}?{APIToken}");
            try
            {
                var request = GetAsync(uri);
                request.Wait();
                var result = request.Result;
                if (result.IsSuccessStatusCode)
                {
                    var symbols = result.Content.ReadAsStringAsync().Result;
                    var objects = JsonConvert.DeserializeObject<List<T>>(symbols);
                    return objects;

                }
                else {
                    return new List<T>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<T>();
            }


        }
    }
}
