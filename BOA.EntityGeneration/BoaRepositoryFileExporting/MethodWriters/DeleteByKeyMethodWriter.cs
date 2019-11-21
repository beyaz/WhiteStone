using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class DeleteByKeyMethodWriter
    {

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo = TableInfo[context];
            var deleteByKeyInfo = DeleteInfoCreator.Create(tableInfo);
            var tableNamingPattern = context.Get(TableNamingPattern);
            
            

            var sqlParameters     = deleteByKeyInfo.SqlParameters;
            var schemaName = context.Get(SchemaName);


            var tableName         = tableInfo.TableName;
            var callerMemberPath = $"{context.Get(NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Delete";

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{schemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            
            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Delete({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            sb.AppendLine();
            sb.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            sb.AppendLine();
            sb.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        
        #endregion
    }
}