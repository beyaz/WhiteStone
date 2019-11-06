using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.ScriptModel.Creators
{
    public class InsertInfoCreator
    {
        #region Public Methods
        public InsertInfo Create(TableInfo tableInfo)
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
        protected virtual IReadOnlyList<IColumnInfo> GetColumnsWillBeInsert(TableInfo tableInfo)
        {
            var excludedColumnNames = new List<string>();

            if (tableInfo.HasIdentityColumn)
            {
                excludedColumnNames.Add(tableInfo.IdentityColumn.ColumnName);
            }

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }

        string GetSql(TableInfo tableInfo, IReadOnlyList<IColumnInfo> columnsWillBeInsert)
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