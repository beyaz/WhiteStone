using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class AllSchemaInOneClassRepositoryFileExporter : ContextContainer
    {
        #region Fields
        readonly PaddedStringBuilder file = new PaddedStringBuilder();
        #endregion

        #region Properties
        AllSchemaInOneClassRepositoryNamingPatternContract AllSchemaInOneClassRepositoryNamingPattern => Context.AllSchemaInOneClassRepositoryNamingPattern;
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
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

        void AddAssemblyReferencesToProject()
        {
            Context.RepositoryAssemblyReferences.AddRange(AllSchemaInOneClassRepositoryNamingPattern.ExtraAssemblyReferences);
        }
        #region Methods
        void BeginClass()
        {
            file.AppendLine($"public sealed class {AllSchemaInOneClassRepositoryNamingPattern.ClassName}");
            file.OpenBracket();

            file.AppendLine("readonly IUnitOfWork unitOfWork;");
            file.AppendLine();
            file.AppendLine($"public {AllSchemaInOneClassRepositoryNamingPattern.ClassName}(IUnitOfWork unitOfWork)");
            file.OpenBracket();
            file.AppendLine("this.unitOfWork = unitOfWork;");
            file.CloseBracket();
        }

        void BeginNamespace()
        {
            file.BeginNamespace(AllSchemaInOneClassRepositoryNamingPattern.NamespaceName);
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

        void WriteClassMethods()
        {
            WriteSelectByKeyMethod();
        }

        void WriteEmbeddedClasses()
        {
            var path = Path.GetDirectoryName(typeof(AllSchemaInOneClassRepositoryFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "AllSchemaInOneClassRepositoryFileEmbeddedCodes.txt";

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
            foreach (var line in AllSchemaInOneClassRepositoryNamingPattern.UsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}