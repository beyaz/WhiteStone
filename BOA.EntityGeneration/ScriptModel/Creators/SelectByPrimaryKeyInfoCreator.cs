using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.ScriptModelCreation
{
    public static class SelectByPrimaryKeyInfoCreator
    {
        #region Public Methods
        public static SelectByPrimaryKeyInfo Create(TableInfo tableInfo)
        {
            var parameters = tableInfo.PrimaryKeyColumns;

            return new SelectByPrimaryKeyInfo
            {
                Sql           = GetSql(tableInfo,parameters),
                SqlParameters = parameters
            };
        }

        static string GetSql(TableInfo tableInfo, IReadOnlyList<ColumnInfo> parameters)
        {
            var sb = new PaddedStringBuilder();


            sb.AppendLine("SELECT");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, tableInfo.Columns.Select(c => $"[{c.ColumnName}]")));
            sb.AppendLine();
            sb.AppendLine($"FROM [{tableInfo.SchemaName}].[{tableInfo.TableName}] WITH (NOLOCK)");
            sb.PaddingCount--;
            sb.AppendLine("WHERE");
            sb.PaddingCount++;

            sb.AppendAll(string.Join(" AND " + Environment.NewLine, parameters.Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;


            return sb.ToString();
        }

        #endregion
    }
}