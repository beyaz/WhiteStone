using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class GetUpdatePart:GeneratorBase
    {

        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetUpdateSql());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetUpdateParameters());

            return sb.ToString();
        }


        string GetUpdateSql()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationSave}.GetUpdateSql()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return @\"");

            sb.AppendLine($"UPDATE [{TableInfo.SchemaName}].[{TableInfo.TableName}] SET");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeUpdate().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("WHERE");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetWhereParameters().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;

            sb.AppendLine("\";");

            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.PaddingCount++;

            return sb.ToString();
        }


        #region Public Methods
        string GetUpdateParameters()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationSave}.GetUpdateParameters(ExecutionScope context)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetSqlInputParameters().Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}