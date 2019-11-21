using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.BoaRepositoryFileExporting.BoaRepositoryFileExporter;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectByIndexMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var file               = File[context];
            var tableInfo          = TableInfo[context];
            var typeContractName   = TableEntityClassNameForMethodParametersInRepositoryFiles[context];
            var tableNamingPattern = context.Get(TableNamingPattern);
            var namingPattern      = context.Get(NamingPattern);

            foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>( CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }

            foreach (var indexIdentifier in tableInfo.NonUniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }
        }
        #endregion
    }
}