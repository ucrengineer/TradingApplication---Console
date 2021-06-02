using System;
using System.Collections.Generic;
using System.Text;
using TradingApplication___Console.Calculations;
using TradingApplication___Console.Models;
using System.Linq;
using TradingApplication___Console.GenericMethods;
using TradingApplication___Console.GenericMethods.Interface;
using TradingApplication___Console.Calculations.Interface;
using TradingApplication___Console.Technicals.Interface;
using Microsoft.Extensions.Logging;
using TradingApplication___Console.DAL.Interface;
using System.Threading.Tasks;

namespace TradingApplication___Console.Technicals
{
    public class TechnicalData : ITechnicalData
    {
        private readonly IGenericPropertyAction _propertyAction;
        private readonly ICalculation _calculation;
        private readonly ILogger<TechnicalData> _log;
        private readonly ITechnicalsRespository _technicalsRespository;

        public TechnicalData(IGenericPropertyAction genericPropertyAction, ICalculation calculation, ILogger<TechnicalData> log, ITechnicalsRespository technicalsRespository)
        {
            _propertyAction = genericPropertyAction;
            _calculation = calculation;
            _log = log;
            _technicalsRespository = technicalsRespository;
        }

        public async Task GetTechnicalsAsync<T> (T t)
        {
            var EODHolder = new Dictionary<string, List<EOD>>()
            {
                {"MA_10", new List<EOD>() },
                {"MA_50", new List<EOD>() },
                {"MA_150", new List<EOD>() },
                {"MA_200", new List<EOD>() },
                {"V_QTR_YEAR", new List<EOD>() },
                {"V_HALF_YEAR", new List<EOD>() },
                {"V_YEAR", new List<EOD>() },
                {"P_QTR_YEAR", new List<EOD>() },
                {"P_HALF_YEAR", new List<EOD>() },
                {"P_YEAR", new List<EOD>() },
                {"RS_QTR_YEAR", new List<EOD>() },
                {"RS_HALF_YEAR", new List<EOD>() },
                {"RS_YEAR", new List<EOD>() }

            };
            var TechnicalList = new List<Technical>();

            var eods = await _propertyAction.GenericGetValueAsync(t, "EODs");
            var type = await _propertyAction.GenericGetValueAsync(t, "Type");

            //var EODs = (List<EOD>)_propertyAction.GenericGetValue(t, "EODs");
            //var tType = (Models.Type)_propertyAction.GenericGetValue(t, "Type");
            var tType = (Models.Type)type;
            var EODs = (List<EOD>)eods;
            foreach(var eod in EODs)
            {
                foreach(var holder in EODHolder.Keys)
                {
                    EODHolder[holder].Add(eod);
                }
                if (EODHolder["MA_10"].Count > 10) { EODHolder["MA_10"].RemoveAt(0); };
                if (EODHolder["MA_50"].Count > 50) { EODHolder["MA_50"].RemoveAt(0); };
                if (EODHolder["MA_150"].Count > 150) { EODHolder["MA_150"].RemoveAt(0); };
                if (EODHolder["MA_200"].Count > 200) { EODHolder["MA_200"].RemoveAt(0); };
                if (EODHolder["V_QTR_YEAR"].Count > 64) { EODHolder["V_QTR_YEAR"].RemoveAt(0); };
                if (EODHolder["V_HALF_YEAR"].Count > 127) { EODHolder["V_HALF_YEAR"].RemoveAt(0); };
                if (EODHolder["V_YEAR"].Count > 254) { EODHolder["V_YEAR"].RemoveAt(0); };
                if (EODHolder["P_QTR_YEAR"].Count > 64) { EODHolder["P_QTR_YEAR"].RemoveAt(0); };
                if (EODHolder["P_HALF_YEAR"].Count > 127) { EODHolder["P_HALF_YEAR"].RemoveAt(0); };
                if (EODHolder["P_YEAR"].Count > 254) { EODHolder["P_YEAR"].RemoveAt(0); };
                if (EODHolder["RS_QTR_YEAR"].Count > 64) { EODHolder["RS_QTR_YEAR"].RemoveAt(0); };
                if (EODHolder["RS_HALF_YEAR"].Count > 127) { EODHolder["RS_HALF_YEAR"].RemoveAt(0); };
                if (EODHolder["RS_YEAR"].Count > 254) { EODHolder["RS_YEAR"].RemoveAt(0); };

                #region call calculation methods
                try
                {
                    Technical technical = new Technical
                    {

                        TECH_DATE = eod.date,
                        MA_10 = await _calculation.CalculateMovingAverageAsync(EODHolder["MA_10"], 10),
                        MA_50 = await _calculation.CalculateMovingAverageAsync(EODHolder["MA_50"], 50),
                        MA_150 = await _calculation.CalculateMovingAverageAsync(EODHolder["MA_150"], 150),
                        MA_200 = await _calculation.CalculateMovingAverageAsync(EODHolder["MA_200"], 200),
                        V_QTR_YEAR = await _calculation.CalculateVolatilityAsync(EODHolder["V_QTR_YEAR"], 63),
                        V_HALF_YEAR = await _calculation.CalculateVolatilityAsync(EODHolder["V_HALF_YEAR"], 126),
                        V_YEAR = await _calculation.CalculateVolatilityAsync(EODHolder["V_YEAR"], 253),
                        P_QTR_YEAR = await _calculation.CalculateResultsAsync(EODHolder["P_QTR_YEAR"], 63),
                        P_HALF_YEAR = await _calculation.CalculateResultsAsync(EODHolder["P_HALF_YEAR"], 126),
                        P_YEAR = await _calculation.CalculateResultsAsync(EODHolder["P_YEAR"], 253),
                        RS_QTR_YEAR = await _calculation.CalculateRelativeStrengthAsync(EODHolder["RS_QTR_YEAR"], 63),
                        RS_HALF_YEAR = await _calculation.CalculateRelativeStrengthAsync(EODHolder["RS_HALF_YEAR"], 126),
                        RS_YEAR = await _calculation.CalculateRelativeStrengthAsync(EODHolder["RS_YEAR"], 253),

                    };

                    switch (tType)
                    {
                        case (Models.Type.Commodity):
                            {
                                var market = await _propertyAction.GenericGetValueAsync(t, "Code");
                                technical.MARKET = market.ToString();
                                break;
                            }
                        case (Models.Type.Stock):
                            {
                                var ticker = await _propertyAction.GenericGetValueAsync(t, "Code");
                                technical.TICKER = ticker.ToString();
                                break;
                            }
                    }




                    TechnicalList.Add(technical);
                    //await _technicalsRespository.UpdateTechnicalsAsync(technical);

                    #endregion


                }
                catch (Exception e)
                {
                    _log.LogInformation("Calculation error: {error}",e.Message);
                    TechnicalList.Add(default(Technical));
                }


            }

            await _propertyAction.GenericSetValueAsync(t, "Technicals", TechnicalList);



        }


        //public Task GetTechnicalsAsync<T>(T t)
        //{
        //    return Task.Run(() => GetTechnicals(t));
        //}



    }
}
