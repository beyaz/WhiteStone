using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
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
            var sb               = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo        = context.Get(TableInfo);
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);
            var tableNamingPattern = context.Get(TableNamingPattern);
            var callerMemberPath   = $"{context.Get(NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByKey";

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

      
            

            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            sb.AppendLine();
            sb.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

       
        #endregion
    }
}