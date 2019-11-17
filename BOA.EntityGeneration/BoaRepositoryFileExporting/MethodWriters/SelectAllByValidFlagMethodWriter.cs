using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class SelectAllByValidFlagMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb               = context.Get(Data.BoaRepositoryFile);
            var tableInfo        = context.Get(Data.TableInfo);
            var typeContractName = context.Get(Data.TableEntityClassNameForMethodParametersInRepositoryFiles);
            var tableNamingPattern = context.Get(Data.TableNamingPattern);
            

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName} where ValidFlag is true.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            sb.OpenBracket();

            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByValidFlag();");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteReaderToList<{typeContractName}>(this, \"{context.Get(Data.NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Select\", sqlInfo, ReadContract);");

            sb.CloseBracket();
        }
        #endregion
    }
}