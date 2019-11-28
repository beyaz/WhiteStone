using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting
{
    class BoaRepositoryFileExporter : ContextContainer
    {
        #region Fields
        readonly PaddedStringBuilder file = new PaddedStringBuilder();
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
            file.BeginNamespace(NamingPattern.RepositoryNamespace);
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
            const string fileName = "Boa.cs";

            var sourceCode = file.ToString();

            ProcessInfo.Text = "Exporting Boa repository...";

            Context.RepositoryProjectSourceFileNames.Add(fileName);

            FileSystem.WriteAllText(NamingPattern.RepositoryProjectDirectory + fileName, sourceCode);
        }

        void WriteClass()
        {
            ContractCommentInfoCreator.Write(file, TableInfo);
            file.AppendLine($"public sealed class {TableNamingPattern.BoaRepositoryClassName} : ObjectHelper");
            file.OpenBracket();

            ContractCommentInfoCreator.Write(file, TableInfo);
            file.AppendLine($"public {TableNamingPattern.BoaRepositoryClassName}(ExecutionDataContext context) : base(context) {{ }}");

            WriteInsertMethod();
            WriteSelectByKeyMethod();
            WriteSelectByIndexMethods();
            WriteSelectAllMethod();
            WriteSelectAllByValidFlagMethod();
            WriteUpdateByKeyMethod();
            WriteDeleteByKeyMethod();

            file.CloseBracket();
        }

        void WriteDeleteByKeyMethod()
        {
            if (!TableInfo.IsSupportSelectByKey)
            {
                return;
            }

            var deleteByKeyInfo = DeleteInfoCreator.Create(TableInfo);

            var sqlParameters = deleteByKeyInfo.SqlParameters;

            var tableName = TableInfo.TableName;

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.Delete";

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment}Deletes only one record from '{SchemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Delete({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
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

            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.Insert";

            var insertInfo = new InsertInfoCreator().Create(TableInfo);

            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
            foreach (var sequenceInfo in TableInfo.SequenceList)
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

            foreach (var sequenceInfo in TableInfo.SequenceList)
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

                var columnInfo = TableInfo.Columns.First(x => x.ColumnName == sequenceInfo.TargetColumnName);
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
                if (Config.DefaultValuesForInsertMethod != null)
                {
                    var contractInitializations = new List<string>();

                    foreach (var columnInfo in insertInfo.SqlParameters)
                    {
                        var contractInstancePropertyName = columnInfo.ColumnName.ToContractName();
                        var contractInstanceName         = contractParameterName;

                        var map = ConfigurationDictionaryCompiler.Compile(Config.DefaultValuesForInsertMethod, new Dictionary<string, string>
                        {
                            {nameof(contractInstanceName), contractInstanceName},
                            {nameof(contractInstancePropertyName), contractInstancePropertyName}
                        });

                        var key = columnInfo.ColumnName;
                        if (map.ContainsKey(key))
                        {
                            contractInitializations.Add(map[key]);
                            continue;
                        }

                        key = columnInfo.DotNetType + ":" + columnInfo.ColumnName;
                        if (map.ContainsKey(key))
                        {
                            contractInitializations.Add(map[key]);
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
            }

            file.AppendLine();
            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Insert({contractParameterName});");
            file.AppendLine();

            file.AppendLine();
            if (TableInfo.HasIdentityColumn)
            {
                file.AppendLine("var response = this.ExecuteScalar<int>(CallerMemberPath, sqlInfo);");
                file.AppendLine();
                file.AppendLine($"{contractParameterName}.{TableInfo.IdentityColumn.ColumnName.ToContractName()} = response.Value;");
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
            if (!TableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                return;
            }

            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.SelectByValidFlag";

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects all records in table {TableInfo.SchemaName}{TableInfo.TableName} where ValidFlag is true.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            file.OpenBracket();

            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByValidFlag();");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.CloseBracket();
        }

        void WriteSelectAllMethod()
        {
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.Select";

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects all records from table '{SchemaName}.{TableInfo.TableName}'.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            file.OpenBracket();

            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Select();");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.CloseBracket();
        }

        void WriteSelectByIndexMethods()
        {
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            foreach (var indexIdentifier in TableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(TableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>( CallerMemberPath, sqlInfo, {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }

            foreach (var indexIdentifier in TableInfo.NonUniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(TableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }
        }

        void WriteSelectByKeyMethod()
        {
            if (!TableInfo.IsSupportSelectByKey)
            {
                return;
            }

            var typeContractName       = TableEntityClassNameForMethodParametersInRepositoryFiles;
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(TableInfo);

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.SelectByKey";

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.PaddingCount--;
            file.AppendLine("}");
        }

        void WriteUpdateByKeyMethod()
        {
            if (!TableInfo.IsSupportSelectByKey)
            {
                return;
            }

            const string contractParameterName = "contract";

            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.Update";

            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(TableInfo);

            file.AppendLine();
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

            if (updateInfo.SqlParameters.Any())
            {
                if (Config.DefaultValuesForUpdateByKeyMethod != null)
                {
                    var contractInitializations = new List<string>();

                    foreach (var columnInfo in updateInfo.SqlParameters)
                    {
                        var contractInstancePropertyName = columnInfo.ColumnName.ToContractName();
                        var contractInstanceName         = contractParameterName;

                        var map = ConfigurationDictionaryCompiler.Compile(Config.DefaultValuesForUpdateByKeyMethod, new Dictionary<string, string>
                        {
                            {nameof(contractInstanceName), contractInstanceName},
                            {nameof(contractInstancePropertyName), contractInstancePropertyName}
                        });

                        var key = columnInfo.ColumnName;
                        if (map.ContainsKey(key))
                        {
                            contractInitializations.Add(map[key]);
                            continue;
                        }

                        key = columnInfo.DotNetType + ":" + columnInfo.ColumnName;
                        if (map.ContainsKey(key))
                        {
                            contractInitializations.Add(map[key]);
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
            }

            file.AppendLine();
            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Update({contractParameterName});");
            file.AppendLine();

            file.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");

            file.CloseBracket();
        }

        void WriteUsingList()
        {
            foreach (var line in NamingPattern.BoaRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}