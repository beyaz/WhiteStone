using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectAllByValidFlagMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb               = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo        = context.Get(Data.TableInfo);
            var typeContractName = context.Get(Data.TableEntityClassNameForMethodParametersInRepositoryFiles);
            var tableNamingPattern = context.Get(TableNamingPattern);
            var callerMemberPath = $"{context.Get(NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByValidFlag";

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName} where ValidFlag is true.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            sb.OpenBracket();

            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByValidFlag();");
            sb.AppendLine();
            sb.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            sb.CloseBracket();
        }
        #endregion
    }
}