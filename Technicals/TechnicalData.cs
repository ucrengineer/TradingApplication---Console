using System;
using System.Collections.Generic;
using System.Text;
using TradingApplication___Console.Calculations;
using TradingApplication___Console.Models;
using System.Linq;
using TradingApplication___Console.GenericMethods;

namespace TradingApplication___Console.Technicals
{
    public class TechnicalData : Calculation
    {
        private readonly GenericPropertyAction _propertyAction;
        public TechnicalData(GenericPropertyAction genericPropertyAction)
        {
            _propertyAction = genericPropertyAction;
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

                Technical technical = new Technical
                {
                    DATE = eod.date,
                    MA_10 = CalculateMovingAverage(EODHolder["MA_10"], 10),
                    MA_50 = CalculateMovingAverage(EODHolder["MA_50"], 50),
                    MA_150 = CalculateMovingAverage(EODHolder["MA_150"], 150),
                    MA_200 = CalculateMovingAverage(EODHolder["MA_200"], 200),
                    V_QTR_YEAR = CalculateVolatility(EODHolder["V_QTR_YEAR"], 63),
                    V_HALF_YEAR = CalculateVolatility(EODHolder["V_HALF_YEAR"], 126),
                    V_YEAR = CalculateVolatility(EODHolder["V_YEAR"], 253),
                    P_QTR_YEAR = CalculateResults(EODHolder["P_QTR_YEAR"], 63),
                    P_HALF_YEAR = CalculateResults(EODHolder["P_HALF_YEAR"], 126),
                    P_YEAR = CalculateResults(EODHolder["P_YEAR"], 253),
                    RS_QTR_YEAR = CalculateRelativeStrength(EODHolder["RS_QTR_YEAR"], 63),
                    RS_HALF_YEAR = CalculateRelativeStrength(EODHolder["RS_HALF_YEAR"], 126),
                    RS_YEAR = CalculateRelativeStrength(EODHolder["RS_YEAR"], 253),

                };

                TechnicalList.Add(technical);

            }

           
            return TechnicalList;



        }
    
    }
}
