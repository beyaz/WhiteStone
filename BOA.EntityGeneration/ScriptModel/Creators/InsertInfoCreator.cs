using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel.Interfaces;

namespace BOA.EntityGeneration.ScriptModel.Creators
{
    public class InsertInfoCreator
    {
        public IReadOnlyList<string> ExcludedColumnNames { get; set; }

        #region Public Methods
        public InsertInfo Create(ITableInfo tableInfo)
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
        IReadOnlyList<IColumnInfo> GetColumnsWillBeInsert(ITableInfo tableInfo)
        {
            var excludedColumnNames = new List<string>();
            if (ExcludedColumnNames != null)
            {
                excludedColumnNames.AddRange(ExcludedColumnNames);
            }

            if (tableInfo.HasIdentityColumn)
            {
                excludedColumnNames.Add(tableInfo.IdentityColumn.ColumnName);
            }

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }

        static string GetSql(ITableInfo tableInfo, IReadOnlyList<IColumnInfo> columnsWillBeInsert)
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