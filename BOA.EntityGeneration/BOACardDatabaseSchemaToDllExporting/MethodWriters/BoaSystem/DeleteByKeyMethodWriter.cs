using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class DeleteByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext businessClassWriterContext)
        {
            var sqlParameters     = businessClassWriterContext.DeleteByKeyInfo.SqlParameters;
            var schemaName        = businessClassWriterContext.TableInfo.SchemaName;
            var tableName         = businessClassWriterContext.TableInfo.TableName;
            var sharedClassConfig = businessClassWriterContext.SharedClassConfig;

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{schemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            
            sb.AppendLine($"var sqlInfo = {schemaName}_Core.{tableName.ToContractName()}.{sharedClassConfig.MethodNameOfDeleteByKey}({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteNonQuery(this, \"{businessClassWriterContext.BusinessClassNamespace}.{businessClassWriterContext.ClassName}.Delete\", sqlInfo);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}