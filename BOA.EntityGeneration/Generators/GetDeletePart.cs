using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;

namespace BOA.EntityGeneration.Generators
{
    class GetDeletePart : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetDeleteSql());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetDeleteParameters());

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetDeleteParameters()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationDelete}.DeleteSqlParameters");

            #region property body
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region get
            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetWhereParameters().Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            return sb.ToString();
        }

        string GetDeleteSql()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationDelete}.DeleteSql");

            #region property body
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region get
            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            sb.AppendLine("return @\"");
            sb.PaddingCount++;
            sb.AppendLine($"DELETE FROM [{TableInfo.SchemaName}].[{TableInfo.TableName}] WHERE {string.Join(" , ", TableInfo.GetWhereParameters().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}"))}");
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