using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.BoaRepositoryFileExporting.BoaRepositoryFileExporter;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectAllByValidFlagMethodWriter
    {
        #region Public Methods
        public static void Write(IContext context)
        {
            var file               = File[context];
            var tableInfo          = TableInfo[context];
            var typeContractName   = TableEntityClassNameForMethodParametersInRepositoryFiles[context];
            var tableNamingPattern = TableNamingPattern[context];
            var namingPattern      = context.Get(NamingPattern);

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByValidFlag";

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName} where ValidFlag is true.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            file.OpenBracket();

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByValidFlag();");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.CloseBracket();
        }
        #endregion
    }
}