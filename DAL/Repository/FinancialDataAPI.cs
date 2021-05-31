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
using Microsoft.Extensions.DependencyInjection;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.GenericMethods.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace TradingApplication___Console.DAL
{
    public enum Type
    {
        US,
        COMM
    }
 
    public class FinancialDataAPI : IFinancialDataAPI
    {
        private Uri BaseAddress { get; set; } = new Uri("https://eodhistoricaldata.com/api/");
        private string APIToken { get; set; } = "api_token=5f51fc52bc5dd8.26338537&fmt=json";
        public Type Type { get; set; }
        private readonly IGenericPropertyAction _propertyAction;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FinancialDataAPI> _log;


        public FinancialDataAPI(IGenericPropertyAction genericPropertyAction, HttpClient httpClient, ILogger<FinancialDataAPI> log)
        {
            _propertyAction = genericPropertyAction;
            _httpClient = httpClient;
            _log = log;
        }



        //https://eodhistoricaldata.com/api/exchanges-list/?api_token=YOUR_API_TOKEN&fmt=json
        public List<T> GetExchangeSymbolList<T>()
        {

            
            var uri = new Uri(this.BaseAddress, $"exchange-symbol-list/{Type}?{APIToken}");
            try
            {
                var request = _httpClient.GetAsync(uri);
                request.Wait();
                var result = request.Result;
                if (result.IsSuccessStatusCode)
                {
                    var symbols = result.Content.ReadAsStringAsync().Result;
                    var objects = JsonConvert.DeserializeObject<List<T>>(symbols,
                        new JsonSerializerSettings
                        {
                            Error = delegate(object sender, ErrorEventArgs args)
                            {
                                _log.LogInformation("Derserialize error: {error}", args.ErrorContext.Error.Message);
                            }
                        });
                    return objects;

                }
                else {
                    return new List<T>();
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("GetExchnageSymbolList Error: {error}",e.Message);
                return default(List<T>);
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
                var request = _httpClient.GetAsync(uri);
                request.Wait();
                var result = request.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    var eods = JsonConvert.DeserializeObject<List<EOD>>(data,
                         new JsonSerializerSettings
                         {
                             Error = delegate (object sender, ErrorEventArgs args)
                             {
                                 _log.LogInformation("Derserialize error: {error}", args.ErrorContext.Error.Message);
                             }
                         });
                    eods.OrderBy(x => x.date);
                    _propertyAction.GenericSetValue<T>(t, "EODs", eods);
                }
            }

            catch (Exception e)
            {

                _log.LogInformation("GetEod Error: {error}",e.Message);
                _propertyAction.GenericSetValue<T>(t, "EODs", default(List<EOD>));
            }



        }


    }
}
