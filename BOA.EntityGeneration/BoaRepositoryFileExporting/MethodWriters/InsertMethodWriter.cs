using System;
using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DataAccess;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.BoaRepositoryFileExporting.BoaRepositoryFileExporter;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;
using static BOA.EntityGeneration.Naming.NamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters
{
    static class InsertMethodWriter
    {
        #region Constants
        const string contractParameterName = "contract";
        #endregion

        #region Public Methods
        public static void Write(IContext context)
        {
            var sb                 = File[context];
            var tableInfo          = TableInfo[context];
            var typeContractName   = TableEntityClassNameForMethodParametersInRepositoryFiles[context];
            var tableNamingPattern = TableNamingPattern[context];
            var namingPattern      = context.Get(NamingPattern);

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Insert";

            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                sb.AppendLine($"///{Padding.ForComment} <para>Automatically initialize '{sequenceInfo.TargetColumnName.ToContractName()}' property by using '{sequenceInfo.Name}' sequence.</para>");
            }

            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
            sb.OpenBracket();
            sb.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            sb.AppendLine();

            sb.AppendLine("if (contract == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull(CallerMemberPath);");
            sb.AppendLine("}");

            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                sb.AppendLine();

                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"// Init sequence for {sequenceInfo.TargetColumnName.ToContractName()}");
                sb.AppendLine();
                sb.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {sequenceInfo.Name}\";");
                sb.AppendLine();
                sb.AppendLine($"const string callerMemberPath = \"{callerMemberPath} -> sqlNextSequence -> {sequenceInfo.Name}\";");
                sb.AppendLine();

                sb.AppendLine("var responseSequence = this.ExecuteScalar<object>(callerMemberPath, new SqlInfo {CommandText = sqlNextSequence});");
                sb.AppendLine("if (!responseSequence.Success)");
                sb.AppendLine("{");
                sb.AppendLine("    return this.SequenceFetchError(responseSequence, callerMemberPath);");
                sb.AppendLine("}");
                sb.AppendLine();

                var columnInfo = tableInfo.Columns.First(x => x.ColumnName == sequenceInfo.TargetColumnName);
                if (columnInfo.DotNetType == DotNetTypeName.DotNetInt32 || columnInfo.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                {
                    sb.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt32(responseSequence.Value);");
                }
                else if (columnInfo.DotNetType == DotNetTypeName.DotNetStringName)
                {
                    sb.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToString(responseSequence.Value);");
                }
                else
                {
                    sb.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt64(responseSequence.Value);");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            if (insertInfo.SqlParameters.Any())
            {
                var contractInitializations = new List<string>();

                if (insertInfo.SqlParameters.Any(c => c.ColumnName == Names2.ROW_GUID))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.ROW_GUID.ToContractName()} = Guid.NewGuid().ToString().ToUpper(new System.Globalization.CultureInfo(\"en-US\", false));");
                }

                if (insertInfo.SqlParameters.Any(c => c.ColumnName == Names2.INSERT_DATE))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.INSERT_DATE.ToContractName()} = DateTime.Now;");
                }

                if (insertInfo.SqlParameters.Any(c => c.ColumnName == Names2.INSERT_USER_ID))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.INSERT_USER_ID.ToContractName()} = Context.ApplicationContext.Authentication.UserName;");
                }

                var tokenIdColumn = insertInfo.SqlParameters.FirstOrDefault(c => c.ColumnName == Names2.INSERT_TOKEN_ID);
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

            sb.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Insert({contractParameterName});");
            sb.AppendLine();

            sb.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                sb.AppendLine("var response = this.ExecuteScalar<int>(CallerMemberPath, sqlInfo);");
                sb.AppendLine();
                sb.AppendLine($"{contractParameterName}.{tableInfo.IdentityColumn.ColumnName.ToContractName()} = response.Value;");
                sb.AppendLine();
                sb.AppendLine("return response;");
            }
            else
            {
                sb.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}