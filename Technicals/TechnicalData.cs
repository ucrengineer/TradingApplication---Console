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

namespace TradingApplication___Console.Technicals
{
    public class TechnicalData : ITechnicalData
    {
        private readonly IGenericPropertyAction _propertyAction;
        private readonly ICalculation _calculation;
        private readonly ILogger<TechnicalData> _log;

        public TechnicalData(IGenericPropertyAction genericPropertyAction, ICalculation calculation, ILogger<TechnicalData> log)
        {
            _propertyAction = genericPropertyAction;
            _calculation = calculation;
            _log = log;
        }

        public List<Technical> GetTechnicals<T> (T t)
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

            var EODs = (List<EOD>)_propertyAction.GenericGetValue(t, "EODs");

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
                        MA_10 = _calculation.CalculateMovingAverage(EODHolder["MA_10"], 10),
                        MA_50 = _calculation.CalculateMovingAverage(EODHolder["MA_50"], 50),
                        MA_150 = _calculation.CalculateMovingAverage(EODHolder["MA_150"], 150),
                        MA_200 = _calculation.CalculateMovingAverage(EODHolder["MA_200"], 200),
                        V_QTR_YEAR = _calculation.CalculateVolatility(EODHolder["V_QTR_YEAR"], 63),
                        V_HALF_YEAR = _calculation.CalculateVolatility(EODHolder["V_HALF_YEAR"], 126),
                        V_YEAR = _calculation.CalculateVolatility(EODHolder["V_YEAR"], 253),
                        P_QTR_YEAR = _calculation.CalculateResults(EODHolder["P_QTR_YEAR"], 63),
                        P_HALF_YEAR = _calculation.CalculateResults(EODHolder["P_HALF_YEAR"], 126),
                        P_YEAR = _calculation.CalculateResults(EODHolder["P_YEAR"], 253),
                        RS_QTR_YEAR = _calculation.CalculateRelativeStrength(EODHolder["RS_QTR_YEAR"], 63),
                        RS_HALF_YEAR = _calculation.CalculateRelativeStrength(EODHolder["RS_HALF_YEAR"], 126),
                        RS_YEAR = _calculation.CalculateRelativeStrength(EODHolder["RS_YEAR"], 253),

                    };

                    TechnicalList.Add(technical);
                 #endregion


                }
                catch (Exception e)
                {
                    _log.LogInformation("Calculation error: {error}",e.Message);
                    TechnicalList.Add(default(Technical));
                }


            }

           
            return TechnicalList;



        }
    
    }
}
