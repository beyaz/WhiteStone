using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class DeleteByKeyMethodWriter
    {

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo = context.Get(TableInfo);
            var deleteByKeyInfo = DeleteInfoCreator.Create(tableInfo);
            var tableNamingPattern = context.Get(TableNamingPatternContract.TableNamingPattern);
            
            

            var sqlParameters     = deleteByKeyInfo.SqlParameters;
            var schemaName = context.Get(SchemaName);

            var businessClassNamespace = context.Get(NamingPatternContract.NamingPattern).RepositoryNamespace;

            var tableName         = tableInfo.TableName;

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{schemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            
            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Delete({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteNonQuery(this, \"{businessClassNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Delete\", sqlInfo);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        
        #endregion
    }
}