using System;
using System.Data;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.SQLParser;
using BOA.EntityGeneration;

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

            if (procedureParameter.SqlDataType.StartsWith(SqlDataType.VARCHAR, StringComparison.Ordinal))
            {
                return "''";
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDataType.Int, StringComparison.Ordinal))
            {
                return 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDataType.DateTime, StringComparison.Ordinal) ||
                procedureParameter.SqlDataType.StartsWith(SqlDataType.Date, StringComparison.Ordinal))
            {
                return DateTime.MinValue;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDataType.SmallDateTime, StringComparison.Ordinal))
            {
                return DateTime.Today.AddDays(-1).Date;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDataType.SmallInt, StringComparison.Ordinal))
            {
                return (short) 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDataType.Bit, StringComparison.Ordinal))
            {
                return false;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.BigInt.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return (long) 0;
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Binary.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new byte[0];
            }

            if (procedureParameter.SqlDataType.StartsWith(SqlDbType.Float.ToString(), StringComparison.OrdinalIgnoreCase))
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