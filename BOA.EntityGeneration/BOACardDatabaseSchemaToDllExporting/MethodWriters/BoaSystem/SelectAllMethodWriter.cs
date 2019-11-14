using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class SelectAllMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb               = context.Get(Data.BoaRepositoryFile);
            var tableInfo        = context.Get(Data.TableInfo);
            var typeContractName = context.Get(Data.TableEntityClassNameForMethodParametersInRepositoryFiles);

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records from table '{context.Get(Data.SchemaName)}.{tableInfo.TableName}'.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            sb.OpenBracket();

            sb.AppendLine($"var sqlInfo = {context.Get(Data.SharedRepositoryClassName)}.Select();");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteReaderToList<{typeContractName}>(this, \"{context.Get(Data.BusinessClassNamespace)}.{context.Get(Data.RepositoryClassName)}.Select\", sqlInfo, ReadContract);");

            sb.CloseBracket();
        }
        #endregion
    }
}