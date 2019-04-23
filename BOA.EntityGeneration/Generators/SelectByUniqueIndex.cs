﻿using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;

namespace BOA.EntityGeneration.Generators
{
    class SelectByUniqueIndex : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetSqlPart());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetParametersPart());

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetParametersPart()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationSelectByUniqueIndex}.GetSelectByUniqueIndexParameters(UniqueIndex uniqueIndex)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in Data.UniqueIndexIdentifiers)
            {
                sb.AppendLine($"if (uniqueIndex == {item.Name})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("return new List<Parameter>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                var whereColumns = TableInfo.Columns.Where(x => item.IndexInfo.ColumnNames.Contains(x.ColumnName));

                sb.AppendAll(string.Join("," + Environment.NewLine, whereColumns.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
                sb.AppendLine();

                sb.PaddingCount--;
                sb.AppendLine("};");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("throw new InvalidIndexException();");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetSqlPart()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationSelectByUniqueIndex}.GetSelectByUniqueIndexSql(UniqueIndex uniqueIndex)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in Data.UniqueIndexIdentifiers)
            {
                sb.AppendLine();
                sb.AppendLine($"if (uniqueIndex == {item.Name})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("return @\"");
                sb.PaddingCount++;

                sb.AppendLine("SELECT");
                sb.PaddingCount++;

                sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.Columns.Select(c => $"[{c.ColumnName}]")));
                sb.AppendLine();
                sb.AppendLine($"FROM [{TableInfo.SchemaName}].[{TableInfo.TableName}] WITH (NOLOCK)");
                sb.PaddingCount--;
                sb.AppendLine("WHERE");
                sb.PaddingCount++;

                sb.AppendAll(string.Join(" AND " + Environment.NewLine, item.IndexInfo.ColumnNames.Select(columnName => $"[{columnName}] = @{columnName}")));
                sb.AppendLine();

                sb.PaddingCount--;

                sb.PaddingCount--;
                sb.AppendLine("\";");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("throw new InvalidIndexException();");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}