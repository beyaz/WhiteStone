using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;

namespace BOA.EntityGeneration.Generators
{
    public class GetInsertPart 
    {
        #region Public Methods
        public string TransformText(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetInsertSql(data));
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetInsertParameters(data));

            sb.AppendLine();
            sb.AppendAll(GetHasSequence(data));

            sb.AppendLine();
            sb.AppendAll(GetSequenceName(data));

            sb.AppendLine();
            sb.AppendAll(InitSequence(data));

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetHasSequence(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"bool {Names.ISupportDmlOperationInsert}.HasSequence");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (data.SequenceName.HasValue())
            {
                sb.AppendLine("return true;");
            }
            else
            {
                sb.AppendLine("return false;");
            }
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetInsertParameters(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationInsert}.GetInsertSqlParameters(ExecutionScope context)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, InsertInfoCreator.Create(data.TableInfo).SqlParameters.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetInsertSql(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationInsert}.InsertSql");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            sb.AppendLine("return @\"");

           sb.AppendAll(InsertInfoCreator.Create(data.TableInfo).Sql);
           sb.AppendLine();

            sb.AppendLine("\";");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetSequenceName(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationInsert}.SequenceName");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (data.SequenceName.HasValue())
            {
                sb.AppendLine($"return \"{data.SequenceName}\";");
            }
            else
            {
                sb.AppendLine("return null;");
            }
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string InitSequence(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"void {Names.ISupportDmlOperationInsert}.InitSequence(long nextValue)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (data.SequenceName.HasValue())
            {
                sb.AppendLine("RecordId = nextValue;");
            }
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}