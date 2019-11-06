using System;
using System.Data;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.SQLParser;
using BOA.EntityGeneration;
using BOA.EntityGeneration.DbModel;

namespace BOA.CodeGeneration.Util
{
    static class IProcedureParameterExtensions
    {
        #region Public Methods
        public static object GetDefaultValueForCallingProcedure(this IProcedureParameter procedureParameter)
        {
            if (procedureParameter.IsNullable)
            {
                return null;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.VarChar, StringComparison.Ordinal))
            {
                return "''";
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Int, StringComparison.Ordinal))
            {
                return 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.DateTime, StringComparison.OrdinalIgnoreCase) ||
                procedureParameter.SqlDataType.StartsWith(SqlDbType.Date, StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.MinValue;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.SmallDateTime, StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.AddDays(-1).Date;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.SmallInt, StringComparison.OrdinalIgnoreCase))
            {
                return (short) 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Bit, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.BigInt, StringComparison.OrdinalIgnoreCase))
            {
                return (long) 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Binary, StringComparison.OrdinalIgnoreCase))
            {
                return new byte[0];
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Float, StringComparison.OrdinalIgnoreCase))
            {
                return (float) 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Money.ToString(), StringComparison.OrdinalIgnoreCase) ||
                procedureParameter.SqlDataType.StartsWith(SqlDbType.SmallMoney.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return (decimal) 0;
            }

            throw new NotImplementedException("Not implemented yet." + procedureParameter.SqlDataType);
        }

        public static string GetDotNetType(this IProcedureParameter procedureParameter)
        {
              


            return new SqlDbTypeMap().GetDotNetType(procedureParameter.SqlDataType, procedureParameter.IsNullable);
        }
        #endregion
    }
}