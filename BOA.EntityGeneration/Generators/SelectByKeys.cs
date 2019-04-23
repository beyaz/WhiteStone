﻿using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;

namespace BOA.EntityGeneration.Generators
{
    class SelectByKeys : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetSqlPart());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetParametersPart());
            sb.AppendLine();

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetParametersPart()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationSelectByKey}.SelectByKeySqlParameters");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.PrimaryKeyColumns.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetSqlPart()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationSelectByKey}.SelectByKeySql");

            #region body
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region get
            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region get body
            sb.AppendLine("return @\"");

            sb.AppendLine("SELECT");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.Columns.Select(c => $"[{c.ColumnName}]")));
            sb.AppendLine();
            sb.AppendLine($"FROM [{TableInfo.SchemaName}].[{TableInfo.TableName}] WITH (NOLOCK)");
            sb.PaddingCount--;
            sb.AppendLine("WHERE");
            sb.PaddingCount++;

            sb.AppendAll(string.Join(" AND " + Environment.NewLine, TableInfo.PrimaryKeyColumns.Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;

            sb.AppendLine("\";");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            return sb.ToString();
        }
        #endregion
    }
}