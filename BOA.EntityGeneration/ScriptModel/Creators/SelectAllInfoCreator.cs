using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel.Types;
using BOA.EntityGeneration.ScriptModel.Types;

namespace BOA.EntityGeneration.ScriptModel.Creators
{
    public static class SelectAllInfoCreator
    {
        #region Public Methods
        public static SelectAllInfo Create(TableInfo tableInfo)
        {
            var parameters = tableInfo.PrimaryKeyColumns;

            return new SelectAllInfo
            {
                Sql           = GetSql(tableInfo),
                SqlParameters = new List<ColumnInfo>()
            };
        }

        static string GetSql(TableInfo tableInfo)
        {
            var sb = new PaddedStringBuilder();


            sb.AppendLine("SELECT ");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, tableInfo.Columns.Select(c => "[" + c.ColumnName + "]")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine($"FROM [{tableInfo.SchemaName}].[{tableInfo.TableName}] WITH(NOLOCK)");


            return sb.ToString();
        }

        #endregion
    }
}