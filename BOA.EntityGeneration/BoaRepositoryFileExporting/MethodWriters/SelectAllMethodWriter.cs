using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;
using static BOA.EntityGeneration.Naming.NamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectAllMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb                 = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo          = context.Get(Data.TableInfo);
            var typeContractName   = context.Get(Data.TableEntityClassNameForMethodParametersInRepositoryFiles);
            var tableNamingPattern = context.Get(TableNamingPattern);
            var callerMemberPath   = $"{context.Get(NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Select";

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records from table '{context.Get(Data.SchemaName)}.{tableInfo.TableName}'.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            sb.OpenBracket();

            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Select();");
            sb.AppendLine();
            sb.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            sb.CloseBracket();
        }
        #endregion
    }
}