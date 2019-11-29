using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
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
            var initialValues = new Dictionary<string, string> {{nameof(SchemaName), SchemaName}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(Config.NamingPattern, initialValues);

            _namingPattern = new NamingPatternContract
            {
                NamespaceName           = dictionary[nameof(NamingPatternContract.NamespaceName)],
                ClassName               = dictionary[nameof(NamingPatternContract.ClassName)],
                UsingLines              = dictionary[nameof(NamingPatternContract.UsingLines)].Split('|'),
                ExtraAssemblyReferences = dictionary[nameof(NamingPatternContract.ExtraAssemblyReferences)].Split('|')
            };
        }

        void WriteClassMethods()
        {
            WriteSelectByKeyMethod();
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

            var callerMemberPath = $"{NamingPattern.RepositoryNamespace}.{TableNamingPattern.BoaRepositoryClassName}.SelectByKey";

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine($"public {typeContractName} Get{TableNamingPattern.BoaRepositoryClassName}By({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return unitOfWork.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.PaddingCount--;
            file.AppendLine("}");
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