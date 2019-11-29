using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.SharedFileExporting;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using Config = BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile.AllSchemaInOneClassRepositoryFileExporterConfig;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{
    class FileExporter : ContextContainer
    {
        #region Static Fields
        static readonly Config Config;
        static readonly string EmbeddedCodes;
        #endregion

        #region Fields
        readonly PaddedStringBuilder file = new PaddedStringBuilder();
        NamingPatternContract        _namingPattern;
        #endregion

        #region Constructors
        static FileExporter()
        {
            var resourceDirectoryPath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(AllSchemaInOneClassRepositoryFile)}{Path.DirectorySeparatorChar}";

            EmbeddedCodes = File.ReadAllText($"{resourceDirectoryPath}EmbeddedCodes.txt");
            Config        = YamlHelper.DeserializeFromFile<Config>(resourceDirectoryPath + nameof(AllSchemaInOneClassRepositoryFileExporterConfig) + ".yaml");
        }
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
                UsingLines              = dictionary[nameof(NamingPatternContract.UsingLines)].SplitAndClear("|"),
                ExtraAssemblyReferences = dictionary[nameof(NamingPatternContract.ExtraAssemblyReferences)].SplitAndClear("|")
            };
        }

        void WriteClassMethods()
        {
            WriteSelectByKeyMethod();
            WriteSelectByIndexMethods();
            WriteDeleteByKeyMethod();
            WriteUpdateByKeyMethod();
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
            file.AppendAll(EmbeddedCodes);
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