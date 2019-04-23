using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;

namespace BOA.EntityGeneration.Generators
{
    class GetInsertPart : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetInsertSql());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetInsertParameters());

            sb.AppendLine();
            sb.AppendAll(GetHasSequence());

            sb.AppendLine();
            sb.AppendAll(GetSequenceName());

            sb.AppendLine();
            sb.AppendAll(InitSequence());

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetHasSequence()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"bool {Names.ISupportDmlOperationInsert}.HasSequence");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (Data.SequenceName.HasValue())
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

        string GetInsertParameters()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationInsert}.GetInsertSqlParameters(ExecutionScope context)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeInsert().Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetInsertSql()
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

            sb.AppendLine($"INSERT INTO [{TableInfo.SchemaName}].[{TableInfo.TableName}]");
            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeInsert().Select(c => "[" + c.ColumnName + "]")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine(")");

            sb.AppendLine("VALUES");

            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeInsert().Select(c => "@" + c.ColumnName)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine(")");

            sb.AppendLine("\";");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        string GetSequenceName()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationInsert}.SequenceName");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (Data.SequenceName.HasValue())
            {
                sb.AppendLine($"return \"{Data.SequenceName}\";");
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

        string InitSequence()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"void {Names.ISupportDmlOperationInsert}.InitSequence(long nextValue)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (Data.SequenceName.HasValue())
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