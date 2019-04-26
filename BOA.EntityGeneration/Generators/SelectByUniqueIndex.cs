using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;

namespace BOA.EntityGeneration.Generators
{
    public class SelectByUniqueIndex 
    {
        #region Public Methods
        public string TransformText(GeneratorData data)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetSqlPart(data));
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetParametersPart(data));

            return sb.ToString();
        }
        #endregion

        #region Methods
        string GetParametersPart(GeneratorData Data)
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

                sb.AppendAll(string.Join("," + Environment.NewLine, SelectByIndexInfoCreator.Create(Data.TableInfo, item).SqlParameters.Select(ParameterHelper.ConvertToParameterDeclarationCode)));
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

        string GetSqlPart(GeneratorData Data)
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

                sb.AppendAll(SelectByIndexInfoCreator.Create(Data.TableInfo, item).Sql);
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