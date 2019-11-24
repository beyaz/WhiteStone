using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class BoaRepositoryFileExporter : ContextContainer
    {
        #region Static Fields
        static readonly Property<PaddedStringBuilder> File = Property.Create<PaddedStringBuilder>(nameof(BoaRepositoryFileExporter) + "->" + nameof(File));
        #endregion

        #region Properties
        PaddedStringBuilder sb => File[Context];
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            var customSqlClassGenerator = Create<CustomSqlClassGenerator>();

            AttachEvent(ProfileInfoInitialized, customSqlClassGenerator.InitializeText);

            customSqlClassGenerator.AttachEvents();

            AttachEvent(ProfileInfoInitialized, InitializeOutput);
            AttachEvent(ProfileInfoInitialized, UsingList);
            AttachEvent(ProfileInfoInitialized, EmptyLine);
            AttachEvent(ProfileInfoInitialized, BeginNamespace);
            AttachEvent(ProfileInfoInitialized, WriteEmbeddedClasses);

            AttachEvent(CustomSqlInfoInitialized, WriteBoaRepositoryClass);

            AttachEvent(ProfileInfoRemove, WriteProxyClass);
            AttachEvent(ProfileInfoRemove, EndNamespace);
            AttachEvent(ProfileInfoRemove, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        void BeginNamespace()
        {
            sb.BeginNamespace(profileNamingPattern.RepositoryNamespace);
        }

        void EmptyLine()
        {
            sb.AppendLine();
        }

        void EndNamespace()
        {
            sb.CloseBracket();
        }

        void ExportFileToDirectory()
        {
            processInfo.Text = "Exporting BOA repository.";

            var filePath = profileNamingPattern.RepositoryProjectDirectory + "Boa.cs";

            FileSystem.WriteAllText( filePath, sb.ToString());
        }

        void InitializeOutput()
        {
            File[Context] = new PaddedStringBuilder();
        }

        void UsingList()
        {
            foreach (var line in profileNamingPattern.BoaRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }

        void WriteBoaRepositoryClass()
        {
            var namingPattern = profileNamingPattern;

            var key = $"{namingPattern.RepositoryNamespace}.{customSqlNamingPattern.RepositoryClassName}.Execute";

            var sharedRepositoryClassAccessPath = $"Shared.{customSqlNamingPattern.RepositoryClassName}";

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{customSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {customSqlNamingPattern.RepositoryClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{customSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {customSqlNamingPattern.RepositoryClassName}(ExecutionDataContext context) : base(context) {{}}");

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{customSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");

            var resultContractName     = customSqlNamingPattern.ResultClassName;
            var readContractMethodPath = $"{sharedRepositoryClassAccessPath}.ReadContract";

            if (customSqlInfo.ResultContractIsReferencedToEntity)
            {
                resultContractName     = customSqlNamingPattern.ReferencedEntityAccessPath;
                readContractMethodPath = customSqlNamingPattern.ReferencedEntityReaderMethodPath;

                repositoryAssemblyReferences.Add(customSqlNamingPattern.ReferencedEntityAssemblyPath);
                repositoryAssemblyReferences.Add(customSqlNamingPattern.ReferencedRepositoryAssemblyPath);
            }

            if (customSqlInfo.SqlResultIsCollection)
            {
                sb.AppendLine($"public GenericResponse<List<{resultContractName}>> Execute({customSqlNamingPattern.InputClassName} request)");
                sb.OpenBracket();
                sb.AppendLine($"const string CallerMemberPath = \"{key}\";");
                sb.AppendLine();
                sb.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.CreateSqlInfo(request);");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderToList<{resultContractName}>(CallerMemberPath, sqlInfo, {readContractMethodPath});");
                sb.CloseBracket();
            }
            else
            {
                sb.AppendLine($"public GenericResponse<{resultContractName}> Execute({customSqlNamingPattern.InputClassName} request)");
                sb.OpenBracket();
                sb.AppendLine($"const string CallerMemberPath = \"{key}\";");
                sb.AppendLine();
                sb.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.CreateSqlInfo(request);");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderToContract<{resultContractName}>(CallerMemberPath, sqlInfo, {readContractMethodPath});");

                sb.CloseBracket();
            }

            sb.AppendLine();

            sb.CloseBracket();
        }

        void WriteEmbeddedClasses()
        {
            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        void WriteProxyClass()
        {
            var proxyClass = Context.Get(CustomSqlClassGenerator.Text).ToString();
            sb.AppendAll(proxyClass);
            sb.AppendLine();
        }
        #endregion
    }
}