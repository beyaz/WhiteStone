using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class DeleteByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext data)
        {
            var parameterPart = string.Join(", ", data.DeleteByKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{data.TableInfo.SchemaName}.{data.TableInfo.TableName}' by using '{string.Join(" and ", data.DeleteByKeyInfo.SqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var sqlInfo = GetDeleteInfo({string.Join(", ", data.DeleteByKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))})");

            sb.AppendLine($"return this.ExecuteNonQuery(\"{data.BusinessClassNamespace}.{data.ClassName}.Delete\", sqlInfo);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}