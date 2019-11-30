using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.SharedFileExporting;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using Config = BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile.AllSchemaInOneClassRepositoryFileExporterConfig;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{
    class AllSchemaInOneClassRepositoryFileExporter : ContextContainer
    {
        #region Static Fields
        internal static readonly Config Config = AllSchemaInOneClassRepositoryFileExporterConfig.CreateFromFile();
        #endregion

        #region Fields
        readonly PaddedStringBuilder file = new PaddedStringBuilder();
        NamingPatternContract        _namingPattern;
        #endregion


        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportStarted += InitializeNamingPattern;
            SchemaExportStarted += AddAssemblyReferencesToProject;
            SchemaExportStarted += WriteUsingList;
            SchemaExportStarted += EmptyLine;
            SchemaExportStarted += BeginNamespace;
            SchemaExportStarted += WriteEmbeddedClasses;
            SchemaExportStarted += EmptyLine;
            SchemaExportStarted += BeginClass;

            TableExportStarted += WriteClassMethods;

            SchemaExportFinished += EndClass;
            SchemaExportFinished += EndNamespace;
            SchemaExportFinished += ExportFileToDirectory;
        }
        #endregion

        #region Methods
        void AddAssemblyReferencesToProject()
        {
            Context.RepositoryAssemblyReferences.AddRange(_namingPattern.ExtraAssemblyReferences);
        }

        void BeginClass()
        {
            file.AppendLine($"public sealed class {_namingPattern.ClassName}");
            file.OpenBracket();

            file.AppendLine("readonly IUnitOfWork unitOfWork;");
            file.AppendLine();
            file.AppendLine($"public {_namingPattern.ClassName}(IUnitOfWork unitOfWork)");
            file.OpenBracket();
            file.AppendLine("this.unitOfWork = unitOfWork;");
            file.CloseBracket();
        }

        void BeginNamespace()
        {
            file.BeginNamespace(_namingPattern.NamespaceName);
        }

        void EmptyLine()
        {
            file.AppendLine();
        }

        void EndClass()
        {
            file.CloseBracket();
        }

        void EndNamespace()
        {
            file.EndNamespace();
        }

        void ExportFileToDirectory()
        {
            const string fileName = "AllInOneRepository.cs";

            var sourceCode = file.ToString();

            ProcessInfo.Text = "Exporting All In One Class repository...";

            Context.RepositoryProjectSourceFileNames.Add(fileName);

            FileSystem.WriteAllText(NamingPattern.RepositoryProjectDirectory + fileName, sourceCode);
        }

        void InitializeNamingPattern()
        {
            var initialValues = new Dictionary<string, string>
            {
                {nameof(SchemaName), SchemaName},
                {nameof(NamingPattern.EntityNamespace), NamingPattern.EntityNamespace},
                {nameof(NamingPattern.RepositoryNamespace), NamingPattern.RepositoryNamespace}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(Config.NamingPattern, initialValues);

            _namingPattern = new NamingPatternContract
            {
                NamespaceName           = dictionary[nameof(NamingPatternContract.NamespaceName)],
                ClassName               = dictionary[nameof(NamingPatternContract.ClassName)],
                UsingLines              = dictionary[nameof(NamingPatternContract.UsingLines)].SplitAndClear(","),
                ExtraAssemblyReferences = dictionary[nameof(NamingPatternContract.ExtraAssemblyReferences)].SplitAndClear(",")
            };
        }

        void WriteClassMethods()
        {
            WriteSelectByKeyMethod();
            WriteSelectByIndexMethods();
            WriteDeleteByKeyMethod();
            WriteUpdateByKeyMethod();
            WriteInsertMethod();
        }

        void WriteDeleteByKeyMethod()
        {
            if (!TableInfo.IsSupportSelectByKey)
            {
                return;
            }

            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var methodName = "Delete" + typeContractName;

            var deleteByKeyInfo                  = DeleteInfoCreator.Create(TableInfo);
            var sqlParameters                    = deleteByKeyInfo.SqlParameters;
            var callerMemberPath                 = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.{methodName}";
            var parameterDefinitionPart          = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));
            var sharedMethodInvocationParameters = string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"));
            var sharedRepositoryClassAccessPath  = TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile;

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Deletes only one record by given primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public int {methodName}({parameterDefinitionPart})");
            file.OpenBracket();
            file.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.Delete({sharedMethodInvocationParameters});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine("return unitOfWork.ExecuteNonQuery(CallerMemberPath, sqlInfo);");
            file.CloseBracket();
        }

        void WriteEmbeddedClasses()
        {
            file.AppendAll(Config.EmbeddedCodes);
            file.AppendLine();
        }

        void WriteSelectByIndexMethods()
        {
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var sharedRepositoryClassAccessPath = TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile;

            var camelCasedTableName = TableNamingPattern.BoaRepositoryClassName;

            foreach (var indexIdentifier in TableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(TableInfo, indexIdentifier);

                var sharedRepositoryMethodName = SharedFileExporter.GetMethodName(indexInfo);

                var parameterDefinitionPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "Get" + camelCasedTableName + "By" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public {typeContractName} {methodName}({parameterDefinitionPart})");
                file.OpenBracket();

                var sharedMethodInvocationParameters = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"));

                var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{camelCasedTableName}.{methodName}";

                file.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.{sharedRepositoryMethodName}({sharedMethodInvocationParameters});");
                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
                file.AppendLine();
                file.AppendLine($"return unitOfWork.ExecuteReaderToContract<{typeContractName}>( CallerMemberPath, sqlInfo, {sharedRepositoryClassAccessPath}.ReadContract);");

                file.CloseBracket();
            }

            foreach (var indexIdentifier in TableInfo.NonUniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(TableInfo, indexIdentifier);

                var parameterDefinitionPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "Get" + camelCasedTableName + "By" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{camelCasedTableName}.{methodName}";

                var sharedRepositoryMethodName = SharedFileExporter.GetMethodName(indexInfo);

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public List<{typeContractName}> {methodName}({parameterDefinitionPart})");
                file.OpenBracket();

                var sharedMethodInvocationParameters = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"));

                file.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.{sharedRepositoryMethodName}({sharedMethodInvocationParameters});");
                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
                file.AppendLine();
                file.AppendLine($"return unitOfWork.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {sharedRepositoryClassAccessPath}.ReadContract);");

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

            var sqlParameters = selectByPrimaryKeyInfo.SqlParameters;

            var parameterDefinitionPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            var methodName = "Get" + TableNamingPattern.BoaRepositoryClassName + "By" + string.Join(string.Empty, sqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

            var callerMemberPath = $"{_namingPattern.NamespaceName}.{_namingPattern.ClassName}.{methodName}";

            file.AppendLine();
            file.AppendLine($"public {typeContractName} {methodName}({parameterDefinitionPart})");
            file.OpenBracket();

            var sharedMethodInvocationParameters = string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"));

            var sharedRepositoryClassAccessPath = TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile;

            file.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.SelectByKey({sharedMethodInvocationParameters});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return unitOfWork.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {sharedRepositoryClassAccessPath}.ReadContract);");

            file.CloseBracket();
        }
        const string contractParameterName = "contract";
        void WriteUpdateByKeyMethod()
        {
            if (!TableInfo.IsSupportSelectByKey)
            {
                return;
            }

            var methodName = "Modify" + TableNamingPattern.BoaRepositoryClassName;

            var sharedRepositoryClassAccessPath = TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile;

            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var callerMemberPath = $"{_namingPattern.NamespaceName}.{_namingPattern.ClassName}.{methodName}";

            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(TableInfo);

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Updates only one record by given primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public int {methodName}({typeContractName} {contractParameterName})");
            file.OpenBracket();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine("if (contract == null)");
            file.AppendLine("{");
            file.AppendLine($"    throw new ArgumentNullException(nameof({contractParameterName}));");
            file.AppendLine("}");

            if (updateInfo.SqlParameters.Any())
            {
                BoaRepositoryFileExporter.WriteDefaultValues(file,Config.DefaultValuesForUpdateByKeyMethod, updateInfo.SqlParameters);
            }

            file.AppendLine();
            file.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.Update({contractParameterName});");
            file.AppendLine();
            file.AppendLine("return unitOfWork.ExecuteNonQuery(CallerMemberPath, sqlInfo);");
            file.CloseBracket();
        }


         void WriteInsertMethod()
        {
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;


            var methodName = "Create" + TableNamingPattern.BoaRepositoryClassName;

            var callerMemberPath = $"{_namingPattern.NamespaceName}.{_namingPattern.ClassName}.{methodName}";

            var sharedRepositoryClassAccessPath = TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile;

            var insertInfo = new InsertInfoCreator{ExcludedColumnNames = Config.ExcludedColumnNamesWhenInsertOperation}.Create(TableInfo);

            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
            foreach (var sequenceInfo in TableInfo.SequenceList)
            {
                file.AppendLine($"///{Padding.ForComment} <para>Automatically initialize '{sequenceInfo.TargetColumnName.ToContractName()}' property by using '{sequenceInfo.Name}' sequence.</para>");
            }

            file.AppendLine("/// </summary>");
            file.AppendLine($"public int Insert({typeContractName} {contractParameterName})");
            file.OpenBracket();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();

            file.AppendLine("if (contract == null)");
            file.AppendLine("{");
            file.AppendLine($"    throw new ArgumentNullException(nameof({contractParameterName}));");
            file.AppendLine("}");

            foreach (var sequenceInfo in TableInfo.SequenceList)
            {
                file.AppendLine();

                file.OpenBracket();

                file.AppendLine($"// Init sequence for {sequenceInfo.TargetColumnName.ToContractName()}");
                file.AppendLine();
                file.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {sequenceInfo.Name}\";");
                file.AppendLine();
                file.AppendLine($"const string callerMemberPath = \"{callerMemberPath} -> sqlNextSequence -> {sequenceInfo.Name}\";");
                file.AppendLine();

                file.AppendLine("var responseSequence = unitOfWork.ExecuteScalar<object>(callerMemberPath, new SqlInfo {CommandText = sqlNextSequence});");
                file.AppendLine();

                var columnInfo = TableInfo.Columns.First(x => x.ColumnName == sequenceInfo.TargetColumnName);
                if (columnInfo.DotNetType == DotNetTypeName.DotNetInt32 || columnInfo.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                {
                    file.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt32(responseSequence);");
                }
                else if (columnInfo.DotNetType == DotNetTypeName.DotNetStringName)
                {
                    file.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToString(responseSequence);");
                }
                else
                {
                    file.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt64(responseSequence);");
                }

                file.CloseBracket();
            }

            if (insertInfo.SqlParameters.Any())
            {
                BoaRepositoryFileExporter.WriteDefaultValues(file, Config.DefaultValuesForInsertMethod, insertInfo.SqlParameters);
            }

            file.AppendLine();
            file.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.Insert({contractParameterName});");

            file.AppendLine();
            if (TableInfo.HasIdentityColumn)
            {
                file.AppendLine("var response = unitOfWork.ExecuteScalar<int>(CallerMemberPath, sqlInfo);");
                file.AppendLine();
                file.AppendLine($"{contractParameterName}.{TableInfo.IdentityColumn.ColumnName.ToContractName()} = response;");
                file.AppendLine();
                file.AppendLine("return response;");
            }
            else
            {
                file.AppendLine("return unitOfWork.ExecuteNonQuery(CallerMemberPath, sqlInfo);");
            }

            file.CloseBracket();
        }

        void WriteUsingList()
        {
            foreach (var line in _namingPattern.UsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}