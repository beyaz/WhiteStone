using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class SelectByKeyMethodWriter
    {

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb               = context.Get(BoaRepositoryFile);
            var tableInfo        = context.Get(TableInfo);
            var schemaName       = context.Get(SchemaName);
            var tableName        = tableInfo.TableName;
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);


            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var sqlInfo = {context.Get(SharedRepositoryClassName)}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteReaderToContract<{typeContractName}>(this, \"{context.Get(NamingPattern.Id).RepositoryNamespace}.{context.Get(RepositoryClassName)}.SelectByKey\", sqlInfo, ReadContract);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

       
        #endregion
    }
}