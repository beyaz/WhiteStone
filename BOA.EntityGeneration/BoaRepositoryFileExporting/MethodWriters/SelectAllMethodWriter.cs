using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.BoaRepositoryFileExporting.BoaRepositoryFileExporter;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;
using static BOA.EntityGeneration.Naming.NamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectAllMethodWriter
    {
        #region Public Methods
        public static void Write(Context context)
        {
            var file               = File[context];
            var tableInfo          = TableInfo[context];
            var typeContractName   = TableEntityClassNameForMethodParametersInRepositoryFiles[context];
            var tableNamingPattern = TableNamingPattern[context];
            var namingPattern      = NamingPattern[context];

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Select";

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects all records from table '{context.Get(SchemaName)}.{tableInfo.TableName}'.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            file.OpenBracket();

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Select();");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.CloseBracket();
        }
        #endregion
    }
}