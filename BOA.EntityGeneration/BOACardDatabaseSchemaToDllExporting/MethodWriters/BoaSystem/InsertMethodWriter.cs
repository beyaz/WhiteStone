﻿using System;
using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using InsertInfoCreator = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess.InsertInfoCreator;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class InsertMethodWriter
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
            var businessClassNamespace = context.Get(BusinessClassNamespace);
           
            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                sb.AppendLine($"///{Padding.ForComment} <para>Automatically initialize '{sequenceInfo.TargetColumnName.ToContractName()}' property by using '{sequenceInfo.Name}' sequence.</para>");
            }

            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (contract == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull();");
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
                sb.AppendLine("var commandNextSequence = this.CreateCommand(sqlNextSequence);");
                sb.AppendLine();
                sb.AppendLine("var responseSequence = DBLayer.ExecuteScalar<object>(commandNextSequence);");
                sb.AppendLine("if (!responseSequence.Success)");
                sb.AppendLine("{");
                sb.AppendLine("    return this.SequenceFetchError(responseSequence);");
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


            sb.AppendLine($"var sqlInfo = {context.Get(SharedRepositoryClassName)}.Insert({contractParameterName});");

            




            sb.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                sb.AppendLine("var response = ExecuteScalar<int>(this, CallerMemberPrefix + nameof(Update), sqlInfo);");
                sb.AppendLine();
                sb.AppendLine($"{contractParameterName}.{tableInfo.IdentityColumn.ColumnName.ToContractName()} = response.Value;");
                sb.AppendLine();
                sb.AppendLine("return response;");
            }
            else
            {
                sb.AppendLine($"return ExecuteNonQuery(this, CallerMemberPrefix + nameof(Update), sqlInfo);");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");


        }

      
        #endregion
    }
}