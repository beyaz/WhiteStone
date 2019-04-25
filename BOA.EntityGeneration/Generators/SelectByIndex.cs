using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;

namespace BOA.EntityGeneration.Generators
{
    class SelectByIndex : GeneratorBase
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

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationSelectByIndex}.GetSelectByIndexParameters(Index index)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in Data.NonUniqueIndexIdentifiers)
            {
                sb.AppendLine($"if (index == {item.Name})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("return new List<Parameter>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendAll(string.Join("," + Environment.NewLine, SelectByIndexInfoCreator.Create(TableInfo, item).SqlParameters.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
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

            sb.AppendLine($"string {Names.ISupportDmlOperationSelectByIndex}.GetSelectByIndexSql(Index index)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in Data.NonUniqueIndexIdentifiers)
            {
                sb.AppendLine();
                sb.AppendLine($"if (index == {item.Name})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("return @\"");
                sb.PaddingCount++;

                sb.AppendAll(SelectByIndexInfoCreator.Create(TableInfo, item).Sql);
                sb.AppendLine();

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