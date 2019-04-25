using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;

namespace BOA.EntityGeneration.Generators
{
    class GetUpdatePart : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetUpdateSql());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetUpdateParameters());

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetUpdateParameters()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationUpdate}.GetUpdateSqlParameters(ExecutionScope context)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, UpdateByPrimaryKeyInfoCreator.Create(TableInfo).SqlParameters.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetUpdateSql()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationUpdate}.UpdateSql");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            
            sb.AppendLine("return @\"");

            sb.AppendAll(UpdateByPrimaryKeyInfoCreator.Create(TableInfo).Sql);
            sb.AppendLine();

            sb.AppendLine("\";");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}