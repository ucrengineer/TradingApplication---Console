using Dapper;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using SUBRAT.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.DAL.Repository
{
    public class TechnicalsRepository
    {
        private readonly OracleConnection _connection;
        private readonly ILogger _logger;

        public TechnicalsRepository(ILogger<TechnicalsRepository> logger, IDbConnection connection)
        {
            _connection = (OracleConnection)connection;
            _logger = logger;
        }

        public void UpdateTechnicals(Technical technical)
        {
            string procName = Constants.PackageName + Constants.UpdateTechnicals;
            var varName = "c# name of variable going into the query";
            #region Params
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_ticker", OracleDbType.Varchar2, ParameterDirection.Input, technical.TICKER ?? null);
            dyParam.Add("p_market", OracleDbType.Varchar2, ParameterDirection.Input, technical.MARKET ?? null);
            dyParam.Add("p_date", OracleDbType.Date, ParameterDirection.Input, technical.DATE);

            // continue to add all of the column data.
            dyParam.Add("p_error_desc", OracleDbType.Varchar2, ParameterDirection.Output, null, 1000);

            #endregion
            try
            {
                using (_connection)
                {
                    _logger.LogDebug("using {name}", procName);

                    SqlMapper.Query(_connection, procName, param:dyParam, commandType: CommandType.StoredProcedure);
                    string error_desc = dyParam.GetOutParam("p_error_desc").ToLower();

                    if (error_desc != "success")
                    {
                        var ex = new Exception(error_desc);
                        _logger.LogError(ex.Message);
                        throw ex;
                    }
                    _logger.LogInformation(error_desc);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public Task UpdateTechnicalsAsync(Technical technical)
        {
            return Task.Run(() => UpdateTechnicals(technical));
        }
    }
}
