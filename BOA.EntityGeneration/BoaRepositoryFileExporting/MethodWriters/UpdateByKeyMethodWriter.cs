﻿using System;
using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;


namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
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
            var sb        = context.Get(BoaRepositoryFileExporter.File);
            var tableInfo = context.Get(TableInfo);
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);
            var tableNamingPattern = context.Get(TableNamingPattern);
            var callerMemberPath = $"{context.Get(NamingPattern).RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Update";


            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Updates only one record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Update({typeContractName} {contractParameterName})");
            sb.OpenBracket();
            sb.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            sb.AppendLine();
            sb.AppendLine("if (contract == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull(CallerMemberPath);");
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

            
            

            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Update({contractParameterName});");
            sb.AppendLine();
            
            sb.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");



            sb.CloseBracket();



        }

      
        #endregion
    }
}