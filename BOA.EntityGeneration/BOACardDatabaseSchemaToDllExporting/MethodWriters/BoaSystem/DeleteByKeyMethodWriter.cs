using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class DeleteByKeyMethodWriter
    {

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(BoaRepositoryFile);
            var tableInfo = context.Get(TableInfo);
            var deleteByKeyInfo = DeleteInfoCreator.Create(tableInfo);

            var sqlParameters     = deleteByKeyInfo.SqlParameters;
            var schemaName = context.Get(SchemaName);

            var businessClassNamespace = context.Get(BusinessClassNamespace);

            var tableName         = tableInfo.TableName;

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{schemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            
            sb.AppendLine($"var sqlInfo = {schemaName}_Core.{tableName.ToContractName()}.Delete({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteNonQuery(this, \"{businessClassNamespace}.{context.Get(RepositoryClassName)}.Delete\", sqlInfo);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        
        #endregion
    }
}