using Dapper;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using SUBRAT.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TradingApplication___Console.DAL.Interface;
using TradingApplication___Console.Models;

namespace TradingApplication___Console.DAL.Repository
{
    public class TechnicalsRepository:ITechnicalsRespository
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
            #region Params
            var dyParam = new OracleDynamicParameters();
            //dyParam.Add("id", OracleDbType.Int32, ParameterDirection.Input, technical.ID);
            dyParam.Add("ticker", OracleDbType.Varchar2, ParameterDirection.Input, technical.TICKER);
            dyParam.Add("market", OracleDbType.Clob, ParameterDirection.Input, technical.MARKET ?? null);
            dyParam.Add("tech_date", OracleDbType.Date, ParameterDirection.Input, technical.TECH_DATE);
            dyParam.Add("rs_year", OracleDbType.Decimal, ParameterDirection.Input, technical.RS_YEAR);
            dyParam.Add("rs_half_year", OracleDbType.Decimal, ParameterDirection.Input, technical.RS_HALF_YEAR);
            dyParam.Add("rs_qtr_year", OracleDbType.Decimal, ParameterDirection.Input, technical.RS_QTR_YEAR);
            dyParam.Add("p_year", OracleDbType.Decimal, ParameterDirection.Input, technical.P_YEAR);
            dyParam.Add("p_half_year", OracleDbType.Decimal, ParameterDirection.Input, technical.P_HALF_YEAR);
            dyParam.Add("p_qtr_year", OracleDbType.Decimal, ParameterDirection.Input, technical.P_QTR_YEAR);
            dyParam.Add("v_year", OracleDbType.Decimal, ParameterDirection.Input, technical.V_YEAR);
            dyParam.Add("v_half_year", OracleDbType.Decimal, ParameterDirection.Input, technical.V_HALF_YEAR);
            dyParam.Add("v_qtr_year", OracleDbType.Decimal, ParameterDirection.Input, technical.V_QTR_YEAR);
            dyParam.Add("ma_200", OracleDbType.Double, ParameterDirection.Input, technical.MA_200);
            dyParam.Add("ma_150", OracleDbType.Double, ParameterDirection.Input, technical.MA_150);
            dyParam.Add("ma_50", OracleDbType.Double, ParameterDirection.Input, technical.MA_50);
            dyParam.Add("ma_10", OracleDbType.Double, ParameterDirection.Input, technical.MA_10);
            dyParam.Add("error", OracleDbType.Varchar2, ParameterDirection.Output, null, 1000);

            #endregion
            try
            {
                using (var _conn = new OracleConnection(_connection.ConnectionString))
                {
                    _conn.Open();
                    _logger.LogDebug("using {name}", procName);
                   
                    SqlMapper.Query(_conn, procName, param: dyParam, commandType: CommandType.StoredProcedure);
                    string error_desc = dyParam.GetOutParam("error").ToLower();

                    if (error_desc != "success")
                    {
                        var ex = new Exception(error_desc);
                        _logger.LogError(ex.Message);
                        throw ex;
                    }
                    //_logger.LogInformation(error_desc);
                    _conn.Close();

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
