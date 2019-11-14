using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class SelectByUniqueIndexMethodWriter
    {

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(Data.BoaRepositoryFile);
            var tableInfo = context.Get(Data.TableInfo);
            var businessClassWriterContext = context.Get(Data.BusinessClassWriterContext);
            var schemaName = context.Get(Data.SchemaName);
            var tableName  = businessClassWriterContext.TableInfo.TableName;
            var typeContractName = businessClassWriterContext.TypeContractName;
           
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

                
                sb.AppendLine($"var sqlInfo = {schemaName}_Core.{tableName.ToContractName()}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

                sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteReaderToContract<{typeContractName}>(this, \"{businessClassWriterContext.BusinessClassNamespace}.{businessClassWriterContext.ClassName}.SelectByKey\", sqlInfo, ReadContract);");

                sb.CloseBracket();
            }



        }

      
        #endregion
    }
}