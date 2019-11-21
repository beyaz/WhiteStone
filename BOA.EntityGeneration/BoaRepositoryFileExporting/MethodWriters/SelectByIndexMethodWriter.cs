using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectByIndexMethodWriter
    {

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo = TableInfo[context];
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);
           
            foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                sb.OpenBracket();

                var tableNamingPattern = context.Get(TableNamingPatternContract.TableNamingPattern);
                


                sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
                sb.AppendLine();
                sb.AppendLine($"const string CallerMemberPath = \"{context.Get(NamingPatternContract.NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>( CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                sb.CloseBracket();
            }

            foreach (var indexIdentifier in tableInfo.NonUniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                sb.OpenBracket();

                var tableNamingPattern = context.Get(TableNamingPatternContract.TableNamingPattern);
                

                sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

                sb.AppendLine();
                sb.AppendLine($"const string CallerMemberPath = \"{context.Get(NamingPatternContract.NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                sb.CloseBracket();
            }



        }

      
        #endregion
    }
}