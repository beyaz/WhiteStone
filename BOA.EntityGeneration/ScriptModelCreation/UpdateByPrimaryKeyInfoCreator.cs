﻿using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.ScriptModelCreation
{
    public static class UpdateByPrimaryKeyInfoCreator
    {
        #region Public Methods
        public static UpdateByPrimaryKeyInfo Create(TableInfo tableInfo)
        {
            var whereParameters = tableInfo.PrimaryKeyColumns;

            var columnsWillBeUpdate = GetColumnsWillBeUpdate(tableInfo);

            return new UpdateByPrimaryKeyInfo
            {
                WhereParameters     = whereParameters,
                ColumnsWillBeUpdate = columnsWillBeUpdate,
                Sql                 = GetSql(tableInfo, columnsWillBeUpdate, whereParameters),
                SqlParameters       = columnsWillBeUpdate.Union(whereParameters).ToList()
            };
        }
        #endregion

        #region Methods
        static IReadOnlyList<ColumnInfo> GetColumnsWillBeUpdate(TableInfo tableInfo)
        {
            var excludedColumnNames = tableInfo.PrimaryKeyColumns.Select(x => x.ColumnName).ToList();

            excludedColumnNames.Add(Names.INSERT_DATE);
            excludedColumnNames.Add(Names.INSERT_TOKEN_ID);
            excludedColumnNames.Add(Names.INSERT_USER_ID);
            excludedColumnNames.Add(Names.ROW_GUID);
            excludedColumnNames.Add(Names.INSERT_USER_ID);

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }

        static string GetSql(TableInfo tableInfo, IReadOnlyList<ColumnInfo> columnsWillBeUpdate, IReadOnlyList<ColumnInfo> whereParameters)
        {
            var sb = new PaddedStringBuilder();


            sb.AppendLine($"UPDATE [{tableInfo.SchemaName}].[{tableInfo.TableName}] SET");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, columnsWillBeUpdate.Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("WHERE");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, whereParameters.Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;


            return sb.ToString();
        }
        #endregion
    }
}