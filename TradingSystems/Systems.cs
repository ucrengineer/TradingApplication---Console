using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.GenericMethods.Interface;
using TradingApplication___Console.Models;
using System.Linq;
using TradingApplication___Console.TradingSystems.Interface;

namespace TradingApplication___Console.TradingSystems
{
    public class Systems : ISystems
    {
        private readonly IGenericPropertyAction _genericPropertyAction;
        private readonly ILogger<Systems> _log;
        private readonly IFinancialDataAPI _financialDataAPI;
        public int n { get; set; } = 4;

        public Systems(IGenericPropertyAction genericPropertyAction, ILogger<Systems> log,
            IFinancialDataAPI financialDataAPI)
        {
            _genericPropertyAction = genericPropertyAction;
            _log = log;
            _financialDataAPI = financialDataAPI;
        }

        public async Task<List<TradingSystem>> TheNWeekRuleAsync<T>(T t)
        {
            // assuming the close is every Friday (in futures and stocks)
            // grab all the eods where the date == day.friday
            // put into a list and order by date
            #region set varibles
            var eods = (List<EOD>) await _genericPropertyAction.GenericGetValueAsync(t, "EODs");
            var weekly_eods = eods.Where(x => x.date.DayOfWeek == DayOfWeek.Friday).OrderBy(x => x.date).ToList();
            var i = 0; var trades = new List<TradingSystem>();
            #endregion end set varibles
            #region logic
            for (i=0; i < weekly_eods.Count(); i++)
            {

                if (i > n && weekly_eods[i].adjusted_close > weekly_eods[i - n].adjusted_close)
                {
                    switch (trades.Any())
                    {
                        case true:
                            {
                                if (trades.LastOrDefault().Side == SIDE.Short)
                                {
                                    TradingSystem tradingSystem = new TradingSystem
                                    {
                                        Strategy = "The N Week Rule",
                                        Side = SIDE.Long,
                                        Quantity = 1,
                                        TradeDate = weekly_eods[i].date,
                                        Trade_PL = trades.Any() ? (double)((weekly_eods[i].adjusted_close - trades.LastOrDefault().Trade_Price)/ weekly_eods[i].adjusted_close) * 100 : 0,
                                        Trade_Price =  weekly_eods[i].adjusted_close
                                    };
                                    trades.Add(tradingSystem);


                                }
                                break;
                            }
                        case false:
                            {
                                TradingSystem tradingSystem = new TradingSystem
                                {
                                    Strategy = "The N Week Rule",
                                    Side = SIDE.Long,
                                    Quantity = 1,
                                    TradeDate = weekly_eods[i].date,
                                    Trade_PL = 0,
                                    Trade_Price = weekly_eods[i].adjusted_close
                                };
                                trades.Add(tradingSystem);


                                break;
                            }
                    }

                }
                else if(i > n && weekly_eods[i].adjusted_close < weekly_eods[i - n].adjusted_close)
                {
                    switch (trades.Any())
                    {
                        case true:
                            {
                                if (trades.LastOrDefault().Side == SIDE.Long)
                                {
                                    TradingSystem tradingSystem = new TradingSystem
                                    {
                                        Strategy = "The N Week Rule",
                                        Side = SIDE.Short,
                                        Quantity = 1,
                                        TradeDate = weekly_eods[i].date,
                                        Trade_PL = trades.Any() ? (double)((weekly_eods[i].adjusted_close - trades.LastOrDefault().Trade_Price) / weekly_eods[i].adjusted_close) * 100 : 0,
                                        Trade_Price =  weekly_eods[i].adjusted_close
                                    };
                                    trades.Add(tradingSystem);


                                }
                                break;
                            }
                        case false:
                            {
                                TradingSystem tradingSystem = new TradingSystem
                                {
                                    Strategy = "The N Week Rule",
                                    Side = SIDE.Short,
                                    Quantity = 1,
                                    TradeDate = weekly_eods[i].date,
                                    Trade_PL = 0,
                                    Trade_Price = weekly_eods[i].adjusted_close
                                };
                                trades.Add(tradingSystem);


                                break;
                            }
                    }
                }
            }
            return trades;
            #endregion end logic
            #region logic explained
            /* if eod.current > eod[-4]
             * NEW TRADING_SYSTEM
             *      {
             *     TRADING_SYSTEM.STRATEGY = "n week Rule"
             *     TRADING_SYSTEM.SIDE = "LONG"
             *     TRADING_SYSTEM.QUANTITY = 1
             *     TRADING_SYSTEM.PRICE = eod.adj_close
             *     TRADING_SYSTEM.DATE = eod.DATE[1]
             *     TRADE_PL = null for first, then TRADING_SYSTEM.PRICE[0] - TRADING_SYSTEM.PRICE[-1]
             *      }
             * LIST.ADD(NEW TRADING SYSTEM)
             * if eod.current < eod[-4]
             * NEW TRADING_SYSTEM
             *      {
             *     TRADING_SYSTEM.STRATEGY = "n week Rule"
             *     TRADING_SYSTEM.SIDE = "LONG"
             *     TRADING_SYSTEM.QUANTITY = 1
             *     TRADING_SYSTEM.PRICE = eod.adj_close
             *     TRADING_SYSTEM.DATE = eod.DATE[1]
             *     TRADE_PL = null for first, then TRADING_SYSTEM.PRICE[0] - TRADING_SYSTEM.PRICE[-1]
             *      }
             * LIST.ADD(NEW TRADING_SYSTEM)
             
             */

            #endregion end logic explained


        }
        public async Task<List<TradingSystem>> TheNWeekRuleAndMovingAverageAsync<T>(T t)
        {
            // assuming the close is every Friday (in futures and stocks)
            // grab all the eods where the date == day.friday
            // put into a list and order by date
            #region set varibles
            var eods = (List<EOD>)await _genericPropertyAction.GenericGetValueAsync(t, "EODs");
            var techs = (List<Technical>)await _genericPropertyAction.GenericGetValueAsync(t, "Technicals");
            var weekly_eods = eods.Where(x => x.date.DayOfWeek == DayOfWeek.Friday).OrderBy(x => x.date).ToList();
            var i = 0; var trades = new List<TradingSystem>();
            #endregion end set varibles
            #region logic
            for (i = 0; i < weekly_eods.Count(); i++)
            {
                var currentEOD = eods.Where(x => x.date == weekly_eods[i].date).FirstOrDefault();
                var currentTech = techs.Where(x => x.TECH_DATE == weekly_eods[i].date).FirstOrDefault();

                if (i > n && currentTech.MA_50 != default(decimal) && weekly_eods[i].adjusted_close > weekly_eods[i - n].adjusted_close 
                    && currentEOD.adjusted_close > currentTech.MA_50 && currentEOD.adjusted_close > currentTech.MA_10)
                {
                    switch (trades.Any())
                    {
                        case true:
                            {
                                if (trades.LastOrDefault().Side == SIDE.Short)
                                {
                                    TradingSystem tradingSystem = new TradingSystem
                                    {
                                        Strategy = "The N Week Rule",
                                        Side = SIDE.Long,
                                        Quantity = 1,
                                        TradeDate = weekly_eods[i].date,
                                        Trade_PL = trades.Any() ? (double)((weekly_eods[i].adjusted_close - trades.LastOrDefault().Trade_Price) / weekly_eods[i].adjusted_close) * 100 : 0,
                                        Trade_Price =  weekly_eods[i].adjusted_close
                                    };
                                    trades.Add(tradingSystem);


                                }
                                break;
                            }
                        case false:
                            {
                                TradingSystem tradingSystem = new TradingSystem
                                {
                                    Strategy = "The N Week Rule",
                                    Side = SIDE.Long,
                                    Quantity = 1,
                                    TradeDate = weekly_eods[i].date,
                                    Trade_PL = 0,
                                    Trade_Price = weekly_eods[i].adjusted_close
                                };
                                trades.Add(tradingSystem);


                                break;
                            }
                    }

                }
                else if (i > n && currentTech.MA_50 != default(decimal)&& weekly_eods[i].adjusted_close < weekly_eods[i - n].adjusted_close
                     && currentEOD.adjusted_close < currentTech.MA_50 && currentEOD.adjusted_close < currentTech.MA_10 )
                {
                    switch (trades.Any())
                    {
                        case true:
                            {
                                if (trades.LastOrDefault().Side == SIDE.Long)
                                {
                                    TradingSystem tradingSystem = new TradingSystem
                                    {
                                        Strategy = "The N Week Rule",
                                        Side = SIDE.Short,
                                        Quantity = 1,
                                        TradeDate = weekly_eods[i].date,
                                        Trade_PL = trades.Any() ? (double)((weekly_eods[i].adjusted_close - trades.LastOrDefault().Trade_Price) / weekly_eods[i].adjusted_close) * 100 : 0,
                                        Trade_Price =  weekly_eods[i].adjusted_close
                                    };
                                    trades.Add(tradingSystem);


                                }
                                break;
                            }
                        case false:
                            {
                                TradingSystem tradingSystem = new TradingSystem
                                {
                                    Strategy = "The N Week Rule",
                                    Side = SIDE.Short,
                                    Quantity = 1,
                                    TradeDate = weekly_eods[i].date,
                                    Trade_PL = 0,
                                    Trade_Price = weekly_eods[i].adjusted_close
                                };
                                trades.Add(tradingSystem);


                                break;
                            }
                    }
                }
            }
            return trades;
            #endregion end logic
            #region logic explained
            /* if eod.current > eod[-4]
             * NEW TRADING_SYSTEM
             *      {
             *     TRADING_SYSTEM.STRATEGY = "n week Rule"
             *     TRADING_SYSTEM.SIDE = "LONG"
             *     TRADING_SYSTEM.QUANTITY = 1
             *     TRADING_SYSTEM.PRICE = eod.adj_close
             *     TRADING_SYSTEM.DATE = eod.DATE[1]
             *     TRADE_PL = null for first, then TRADING_SYSTEM.PRICE[0] - TRADING_SYSTEM.PRICE[-1]
             *      }
             * LIST.ADD(NEW TRADING SYSTEM)
             * if eod.current < eod[-4]
             * NEW TRADING_SYSTEM
             *      {
             *     TRADING_SYSTEM.STRATEGY = "n week Rule"
             *     TRADING_SYSTEM.SIDE = "LONG"
             *     TRADING_SYSTEM.QUANTITY = 1
             *     TRADING_SYSTEM.PRICE = eod.adj_close
             *     TRADING_SYSTEM.DATE = eod.DATE[1]
             *     TRADE_PL = null for first, then TRADING_SYSTEM.PRICE[0] - TRADING_SYSTEM.PRICE[-1]
             *      }
             * LIST.ADD(NEW TRADING_SYSTEM)
             
             */

            #endregion end logic explained


        }

    }
}
