using System;
using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class UpdateByKeyMethodWriter
    {
        const string contractParameterName = "contract";

        /// <summary>
        ///     The parameter identifier
        /// </summary>
        const string ParameterIdentifier = "@";

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(BoaRepositoryFile);
            var tableInfo = context.Get(TableInfo);
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);
            var businessClassNamespace = context.Get(NamingPattern.Id).RepositoryNamespace;
           
            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Updates only one record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Update({typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (contract == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull();");
            sb.AppendLine("}");


            sb.AppendLine();

           

         

            if (updateInfo.SqlParameters.Any())
            {
                var contractInitializations = new List<string>();

                if (updateInfo.SqlParameters.Any(c => c.ColumnName == Names2.UPDATE_DATE))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.UPDATE_DATE.ToContractName()} = DateTime.Now;");
                }

                if (updateInfo.SqlParameters.Any(c => c.ColumnName == Names2.UPDATE_USER_ID))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.UPDATE_USER_ID.ToContractName()} = Context.ApplicationContext.Authentication.UserName;");
                }

                var tokenIdColumn = updateInfo.SqlParameters.FirstOrDefault(c => c.ColumnName == Names2.UPDATE_TOKEN_ID);
                if (tokenIdColumn != null)
                {
                    if (tokenIdColumn.DotNetType == DotNetTypeName.DotNetInt32 ||
                        tokenIdColumn.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                    {
                        contractInitializations.Add($"{contractParameterName}.{tokenIdColumn.ColumnName.ToContractName()} = decimal.ToInt32(Context.EngineContext.MainBusinessKey);");
                    }
                    else if (tokenIdColumn.DotNetType == DotNetTypeName.DotNetStringName)
                    {
                        contractInitializations.Add($"{contractParameterName}.{tokenIdColumn.ColumnName.ToContractName()} = Context.EngineContext.MainBusinessKey.ToString();");
                    }
                    else
                    {
                        throw new NotImplementedException(tokenIdColumn.DotNetType);
                    }
                }

                if (contractInitializations.Any())
                {
                    sb.AppendLine();
                    foreach (var item in contractInitializations)
                    {
                        sb.AppendLine(item);
                    }
                }

            }

            sb.AppendLine($"var sqlInfo = {context.Get(SharedRepositoryClassName)}.Update({contractParameterName});");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteNonQuery(this, \"{businessClassNamespace}.{context.Get(RepositoryClassName)}.Update\", sqlInfo);");



            sb.PaddingCount--;
            sb.AppendLine("}");



        }

      
        #endregion
    }
}