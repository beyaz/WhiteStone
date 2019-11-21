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
    static class SelectByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var file                   = File[context];
            var tableInfo              = TableInfo[context];
            var typeContractName       = TableEntityClassNameForMethodParametersInRepositoryFiles[context];
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);
            var tableNamingPattern     = TableNamingPattern[context];
            var namingPattern          = context.Get(NamingPattern);

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByKey";

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.PaddingCount--;
            file.AppendLine("}");
        }
        #endregion
    }
}