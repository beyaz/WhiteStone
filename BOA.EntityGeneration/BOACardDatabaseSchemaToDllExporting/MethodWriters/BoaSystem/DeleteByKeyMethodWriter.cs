using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class DeleteByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, DeleteInfo deleteInfo, ITableInfo tableInfo, string businessClassNamespace, string className)
        {
            var parameterPart = string.Join(", ", deleteInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{tableInfo.SchemaName}.{tableInfo.TableName}' by using '{string.Join(" and ", deleteInfo.SqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var sqlInfo = GetDeleteInfo({string.Join(", ", deleteInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))})");

            sb.AppendLine($"return this.ExecuteNonQuery(\"{businessClassNamespace}.{className}.Delete\", sqlInfo);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}