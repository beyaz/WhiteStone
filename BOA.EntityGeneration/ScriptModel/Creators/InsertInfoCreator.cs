using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel.Types;
using BOA.EntityGeneration.ScriptModel.Types;

namespace BOA.EntityGeneration.ScriptModel.Creators
{
    public static class InsertInfoCreator
    {
        #region Public Methods
        public static InsertInfo Create(TableInfo tableInfo)
        {
            var columnsWillBeInsert = GetColumnsWillBeInsert(tableInfo);

            return new InsertInfo
            {
                Sql           = GetSql(tableInfo, columnsWillBeInsert),
                SqlParameters = columnsWillBeInsert
            };
        }
        #endregion

        #region Methods
        static IReadOnlyList<ColumnInfo> GetColumnsWillBeInsert(TableInfo tableInfo)
        {
            var excludedColumnNames = new List<string>
            {
                Names2.UPDATE_DATE,
                Names2.UPDATE_USER_ID,
                Names2.UPDATE_TOKEN_ID
            };

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }

        static string GetSql(TableInfo tableInfo, IReadOnlyList<ColumnInfo> columnsWillBeInsert)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"INSERT INTO [{tableInfo.SchemaName}].[{tableInfo.TableName}]");
            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, columnsWillBeInsert.Select(c => "[" + c.ColumnName + "]")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine(")");

            sb.AppendLine("VALUES");

            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, columnsWillBeInsert.Select(c => "@" + c.ColumnName)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine(")");

            return sb.ToString();
        }
        #endregion
    }
}