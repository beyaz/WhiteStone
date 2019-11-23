using System;
using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.BoaRepositoryFileExporting.BoaRepositoryFileExporter;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class UpdateByKeyMethodWriter
    {
        #region Constants
        const string contractParameterName = "contract";
        #endregion

        #region Public Methods
        public static void Write(IContext context)
        {
            var file               = File[context];
            var tableInfo          = TableInfo[context];
            var typeContractName   = TableEntityClassNameForMethodParametersInRepositoryFiles[context];
            var tableNamingPattern = TableNamingPattern[context];
            var namingPattern      = NamingPattern[context];

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Update";

            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Updates only one record by primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<int> Update({typeContractName} {contractParameterName})");
            file.OpenBracket();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine("if (contract == null)");
            file.AppendLine("{");
            file.AppendLine("    return this.ContractCannotBeNull(CallerMemberPath);");
            file.AppendLine("}");

            file.AppendLine();

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
                    file.AppendLine();
                    foreach (var item in contractInitializations)
                    {
                        file.AppendLine(item);
                    }
                }
            }

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Update({contractParameterName});");
            file.AppendLine();

            file.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");

            file.CloseBracket();
        }
        #endregion
    }
}