using BOA.Common.Helpers;
using BOA.EntityGeneration.CustomSQLExporting.ContextManagement;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.BoaRepositoryExporting
{
    class BoaRepositoryFileExporter : ContextContainer
    {
        #region Static Fields
        internal static readonly BoaRepositoryFileExporterConfig Config = BoaRepositoryFileExporterConfig.CreateFromFile();
        #endregion

        #region Fields
        readonly PaddedStringBuilder sb = new PaddedStringBuilder();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            var customSqlClassGenerator = Create<CustomSqlClassGenerator>();
            
            customSqlClassGenerator.AttachEvents();

            Context.ProfileInfoInitialized += UsingList;
            Context.ProfileInfoInitialized += EmptyLine;
            Context.ProfileInfoInitialized += BeginNamespace;
            Context.ProfileInfoInitialized += WriteEmbeddedClasses;

            Context.CustomSqlInfoInitialized += WriteBoaRepositoryClass;

            Context.ProfileInfoRemove += () =>
            {
                var proxyClass = customSqlClassGenerator.sb.ToString();
                sb.AppendAll(proxyClass);
                sb.AppendLine();
            };
            Context.ProfileInfoRemove += EndNamespace;
            Context.ProfileInfoRemove += ExportFileToDirectory;
        }
        #endregion

        #region Methods
        void BeginNamespace()
        {
            sb.BeginNamespace(NamingMap.RepositoryNamespace);
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
            ProcessInfo.Text = "Exporting BOA repository.";

            var filePath = Resolve(Config.OutputFilePath);

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

        void UsingList()
        {
            foreach (var line in Config.UsingLines)
            {
                sb.AppendLine(Resolve(line));
            }
        }

        void WriteBoaRepositoryClass()
        {
            var key = $"{NamingMap.RepositoryNamespace}.{NamingMap.RepositoryClassName}.Execute";

            var sharedRepositoryClassAccessPath = $"Shared.{NamingMap.RepositoryClassName}";

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {NamingMap.RepositoryClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {NamingMap.RepositoryClassName}(ExecutionDataContext context) : base(context) {{}}");

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");

            var resultContractName     = NamingMap.ResultClassName;
            var readContractMethodPath = $"{sharedRepositoryClassAccessPath}.ReadContract";

            if (CustomSqlInfo.ResultContractIsReferencedToEntity)
            {
                resultContractName     = ReferencedEntityTypeNamingPattern.ReferencedEntityAccessPath;
                readContractMethodPath = ReferencedEntityTypeNamingPattern.ReferencedEntityReaderMethodPath;

                Context.ExtraAssemblyReferencesForRepositoryProject.Add(ReferencedEntityTypeNamingPattern.ReferencedEntityAssemblyPath);
                Context.ExtraAssemblyReferencesForRepositoryProject.Add(ReferencedEntityTypeNamingPattern.ReferencedRepositoryAssemblyPath);
            }

            if (CustomSqlInfo.SqlResultIsCollection)
            {
                sb.AppendLine($"public GenericResponse<List<{resultContractName}>> Execute({NamingMap.InputClassName} request)");
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
                sb.AppendLine($"public GenericResponse<{resultContractName}> Execute({NamingMap.InputClassName} request)");
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
            sb.AppendAll(Config.EmbeddedCodes);
            sb.AppendLine();
        }
        #endregion
    }
}