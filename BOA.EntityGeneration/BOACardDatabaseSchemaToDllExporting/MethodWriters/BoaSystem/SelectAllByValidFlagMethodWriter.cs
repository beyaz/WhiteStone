using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class SelectAllByValidFlagMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb               = context.Get(Data.BoaRepositoryFile);
            var tableInfo        = context.Get(Data.TableInfo);
            var typeContractName = context.Get(Data.TableEntityClassNameForMethodParametersInRepositoryFiles);

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName} where ValidFlag is true.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            sb.OpenBracket();

            sb.AppendLine($"var sqlInfo = {context.Get(Data.SharedRepositoryClassName)}.SelectByValidFlag();");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteReaderToList<{typeContractName}>(this, \"{context.Get(NamingPattern.Id).RepositoryNamespace}.{context.Get(Data.RepositoryClassName)}.Select\", sqlInfo, ReadContract);");

            sb.CloseBracket();
        }
        #endregion
    }
}