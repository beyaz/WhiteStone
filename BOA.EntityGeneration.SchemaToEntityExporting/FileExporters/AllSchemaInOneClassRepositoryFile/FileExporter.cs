using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{
    class FileExporter : ContextContainer
    {
        #region Fields
        static readonly AllSchemaInOneClassRepositoryFileExporterConfig      Config = AllSchemaInOneClassRepositoryFileExporterConfig.CreateFromFile();
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
                { nameof(NamingPattern.EntityNamespace),NamingPattern.EntityNamespace},
                { nameof(NamingPattern.RepositoryNamespace),NamingPattern.RepositoryNamespace}
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
        }

        void WriteEmbeddedClasses()
        {
            var path = Path.GetDirectoryName(typeof(FileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "AllSchemaInOneClassRepositoryFileEmbeddedCodes.txt";

            file.AppendAll(File.ReadAllText(path));
            file.AppendLine();
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

            var methodName = "Get" + TableNamingPattern.BoaRepositoryClassName +"By"+ string.Join(string.Empty, sqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

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

        void WriteSelectByIndexMethods()
        {
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles;

            var sharedRepositoryClassAccessPath = TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile;

            var camelCasedTableName = TableNamingPattern.BoaRepositoryClassName;

            foreach (var indexIdentifier in TableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(TableInfo, indexIdentifier);

                var sharedRepositoryMethodName = SharedFileExporting.SharedFileExporter.GetMethodName(indexInfo);

                var parameterDefinitionPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "Get" + camelCasedTableName +"By"+ string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));


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

                var methodName = "Get" + camelCasedTableName +"By"+ string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{camelCasedTableName}.{methodName}";

                var sharedRepositoryMethodName = SharedFileExporting.SharedFileExporter.GetMethodName(indexInfo);

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