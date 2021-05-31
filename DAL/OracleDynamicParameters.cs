using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace SUBRAT.DAL
{
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new DynamicParameters();
        private readonly List<OracleParameter> oracleParameters = new List<OracleParameter>();

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, object value = null, int? size = null)
        {
            OracleParameter oracleParameter;
            if (size.HasValue)
            {
                oracleParameter = new OracleParameter(name, oracleDbType, size.Value, value, direction);
            }
            else
            {
                oracleParameter = new OracleParameter(name, oracleDbType, value, direction);
            }

            oracleParameters.Add(oracleParameter);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, OracleCollectionType oracleCollectionType, object value = null, int? size = null)
        {
            OracleParameter oracleParameter;
            if (size.HasValue)
            {
                oracleParameter = new OracleParameter()
                {
                    ParameterName = name,
                    OracleDbType = oracleDbType,
                    CollectionType = oracleCollectionType,
                    Direction = direction,
                    Value = value,
                    Size = size.Value,
                };
            }
            else
            {
                oracleParameter = new OracleParameter()
                {
                    ParameterName = name,
                    OracleDbType = oracleDbType,
                    CollectionType = oracleCollectionType,
                    Direction = direction,
                    Value = value
                };
            }
            oracleParameters.Add(oracleParameter);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, direction);
            oracleParameters.Add(oracleParameter);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)dynamicParameters).AddParameters(command, identity);
            if (command is OracleCommand oracleCommand)
            {
                oracleCommand.Parameters.AddRange(oracleParameters.ToArray());
            }
        }

        /// <summary>
        /// Use this method to get the Error_Desc
        /// </summary>
        /// <param name="param"> Name of string out you want </param>
        /// <returns></returns>
        public string GetOutParam(string param)
        {
            var tmp = (from p in this.oracleParameters.AsEnumerable()
                       where p.ParameterName.ToString() == param
                       select p.Value).FirstOrDefault();
            return tmp.ToString();
        }

        public List<OracleParameter> GetOracleParams()
        {
            return this.oracleParameters;
        }
    }
}
