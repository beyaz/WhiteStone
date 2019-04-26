using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;

namespace BOA.EntityGeneration.Generators
{
    public class GetDeletePart 
    {
        #region Public Methods
        public string TransformText(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetDeleteSql(data));
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetDeleteParameters(data));

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetDeleteParameters(GeneratorData data)
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

            sb.AppendAll(string.Join("," + Environment.NewLine, DeleteInfoCreator.Create(data.TableInfo).SqlParameters.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
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

        string GetDeleteSql(GeneratorData data)
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

            sb.AppendLine(DeleteInfoCreator.Create(data.TableInfo).Sql);
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