using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;
using TradingApplication___Console.GenericMethods;

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
        private readonly GenericPropertyAction _propertyAction;
        public FinancialDataAPI(GenericPropertyAction genericPropertyAction)
        {
            _propertyAction = genericPropertyAction;
        }


        //https://eodhistoricaldata.com/api/exchanges-list/?api_token=YOUR_API_TOKEN&fmt=json
        public List<T> GetExchangeSymbolList<T>()
        {
            var uri = new Uri(this.BaseAddress, $"exchange-symbol-list/{Type}?{APIToken}");
            Console.WriteLine(uri);
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
        //https://eodhistoricaldata.com/api/eod/MCD.US?from=2020-01-05&to=2020-02-10&api_token=OeAFFmMliFG5orCUuwAKQ8l4WWFQ67YX&period=d
        public void GetEod<T>(T t)
        {

            var dateFrom = DateTime.Now.AddYears(-5).ToString("yyyy-MM-d");
            var dateTo = DateTime.Now.ToString("yyyy-MM-d");
            string range = $"from={dateFrom}&to={dateTo}";
            var code = _propertyAction.GenericGetValue<T>(t, "Code");
            var uri = new Uri(this.BaseAddress, $"eod/{code}.{Type}?{range}&{APIToken}&period=d");
            try
            {
                var request = GetAsync(uri);
                request.Wait();
                var result = request.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    var eods = JsonConvert.DeserializeObject<List<EOD>>(data).OrderBy(x => x.date);
                    _propertyAction.GenericSetValue<T>(t, "EODs", eods); 
                }
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e.Message);
                _propertyAction.GenericSetValue<T>(t, "EODs", new List<EOD>());
            }

        }


    }
}
