using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using InsertInfoCreator = BOA.EntityGeneration.DataAccess.InsertInfoCreator;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting
{
    class BoaRepositoryFileExporter : ContextContainer
    {
        #region Fields
        readonly PaddedStringBuilder file;
        #endregion

        #region Constructors
        public BoaRepositoryFileExporter()
        {
            file = new PaddedStringBuilder();
        }
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportStarted += WriteUsingList;
            SchemaExportStarted += EmptyLine;
            SchemaExportStarted += BeginNamespace;
            SchemaExportStarted += WriteEmbeddedClasses;

            TableExportStarted += WriteClass;

            SchemaExportFinished += EndNamespace;
            SchemaExportFinished += ExportFileToDirectory;
        }
        #endregion

        #region Methods
        void BeginNamespace()
        {
            file.BeginNamespace(namingPattern.RepositoryNamespace);
        }

        void EmptyLine()
        {
            file.AppendLine();
        }

        void EndNamespace()
        {
            file.EndNamespace();
        }

        void ExportFileToDirectory()
        {
            var sourceCode = file.ToString();

            processInfo.Text = "Exporting Boa repository...";

            FileSystem.WriteAllText(namingPattern.RepositoryProjectDirectory + "Boa.cs", sourceCode);
        }

        void WriteClass()
        {
            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine($"public sealed class {tableNamingPattern.BoaRepositoryClassName} : ObjectHelper");
            file.AppendLine("{");
            file.PaddingCount++;

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine($"public {tableNamingPattern.BoaRepositoryClassName}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                WriteDeleteByKeyMethod();
            }
            #endregion

            WriteInsertMethod();

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                WriteUpdateByKeyMethod();
            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                WriteSelectByKeyMethod();
            }
            #endregion

            WriteSelectByIndexMethods();

            WriteSelectAllMethod();

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                WriteSelectAllByValidFlagMethod();
            }

            file.CloseBracket();
        }

        void WriteDeleteByKeyMethod()
        {
            var deleteByKeyInfo = DeleteInfoCreator.Create(tableInfo);

            var sqlParameters = deleteByKeyInfo.SqlParameters;

            var tableName = tableInfo.TableName;

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Delete";

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment}Deletes only one record from '{SchemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Delete({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");

            file.PaddingCount--;
            file.AppendLine("}");
        }

        void WriteEmbeddedClasses()
        {
            var path = Path.GetDirectoryName(typeof(BoaRepositoryFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            file.AppendAll(File.ReadAllText(path));
            file.AppendLine();
        }

        void WriteInsertMethod()
        {
            const string contractParameterName = "contract";

            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Insert";

            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                file.AppendLine($"///{Padding.ForComment} <para>Automatically initialize '{sequenceInfo.TargetColumnName.ToContractName()}' property by using '{sequenceInfo.Name}' sequence.</para>");
            }

            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
            file.OpenBracket();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();

            file.AppendLine("if (contract == null)");
            file.AppendLine("{");
            file.AppendLine("    return this.ContractCannotBeNull(CallerMemberPath);");
            file.AppendLine("}");

            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                file.AppendLine();

                file.AppendLine("{");
                file.PaddingCount++;

                file.AppendLine($"// Init sequence for {sequenceInfo.TargetColumnName.ToContractName()}");
                file.AppendLine();
                file.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {sequenceInfo.Name}\";");
                file.AppendLine();
                file.AppendLine($"const string callerMemberPath = \"{callerMemberPath} -> sqlNextSequence -> {sequenceInfo.Name}\";");
                file.AppendLine();

                file.AppendLine("var responseSequence = this.ExecuteScalar<object>(callerMemberPath, new SqlInfo {CommandText = sqlNextSequence});");
                file.AppendLine("if (!responseSequence.Success)");
                file.AppendLine("{");
                file.AppendLine("    return this.SequenceFetchError(responseSequence, callerMemberPath);");
                file.AppendLine("}");
                file.AppendLine();

                var columnInfo = tableInfo.Columns.First(x => x.ColumnName == sequenceInfo.TargetColumnName);
                if (columnInfo.DotNetType == DotNetTypeName.DotNetInt32 || columnInfo.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                {
                    file.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt32(responseSequence.Value);");
                }
                else if (columnInfo.DotNetType == DotNetTypeName.DotNetStringName)
                {
                    file.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToString(responseSequence.Value);");
                }
                else
                {
                    file.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt64(responseSequence.Value);");
                }

                file.PaddingCount--;
                file.AppendLine("}");
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
                    file.AppendLine();
                    foreach (var item in contractInitializations)
                    {
                        file.AppendLine(item);
                    }
                }
            }

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Insert({contractParameterName});");
            file.AppendLine();

            file.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                file.AppendLine("var response = this.ExecuteScalar<int>(CallerMemberPath, sqlInfo);");
                file.AppendLine();
                file.AppendLine($"{contractParameterName}.{tableInfo.IdentityColumn.ColumnName.ToContractName()} = response.Value;");
                file.AppendLine();
                file.AppendLine("return response;");
            }
            else
            {
                file.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");
            }

            file.PaddingCount--;
            file.AppendLine("}");
        }

        void WriteSelectAllByValidFlagMethod()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByValidFlag";

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName} where ValidFlag is true.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            file.OpenBracket();

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByValidFlag();");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.CloseBracket();
        }

        void WriteSelectAllMethod()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Select";

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects all records from table '{SchemaName}.{tableInfo.TableName}'.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            file.OpenBracket();

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Select();");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.CloseBracket();
        }

        void WriteSelectByIndexMethods()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>( CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }

            foreach (var indexIdentifier in tableInfo.NonUniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }
        }

        void WriteSelectByKeyMethod()
        {
            var typeContractName       = tableEntityClassNameForMethodParametersInRepositoryFiles;
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByKey";

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.PaddingCount--;
            file.AppendLine("}");
        }

        void WriteUpdateByKeyMethod()
        {
            const string contractParameterName = "contract";

            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

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

        void WriteUsingList()
        {
            foreach (var line in namingPattern.BoaRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}