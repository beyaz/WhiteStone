using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
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

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationDelete}.GetDeleteParameters(ExecutionScope context)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetWhereParameters().Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetDeleteSql()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationDelete}.GetDeleteSql()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return @\"");
            sb.PaddingCount++;
            sb.AppendLine($"DELETE FROM [{TableInfo.SchemaName}].[{TableInfo.TableName}] WHERE {string.Join(" , ", TableInfo.GetWhereParameters().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}"))}");
            sb.PaddingCount--;
            sb.AppendLine("\";");

            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.PaddingCount++;

            return sb.ToString();
        }
        #endregion
    }
}