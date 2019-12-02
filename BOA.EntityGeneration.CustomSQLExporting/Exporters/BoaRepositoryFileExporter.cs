using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.CustomSQLExporting.Exporters.SharedFileExporting;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class BoaRepositoryFileExporter : ContextContainer
    {
       

        #region Properties
        readonly PaddedStringBuilder sb =new PaddedStringBuilder();
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
            sb.BeginNamespace(ProfileNamingPattern.RepositoryNamespace);
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

            var filePath = ProfileNamingPattern.RepositoryProjectDirectory + "Boa.cs";

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

        

        void UsingList()
        {
            foreach (var line in ProfileNamingPattern.BoaRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }

        void WriteBoaRepositoryClass()
        {
            var namingPattern = ProfileNamingPattern;

            var key = $"{namingPattern.RepositoryNamespace}.{CustomSqlNamingPattern.RepositoryClassName}.Execute";

            var sharedRepositoryClassAccessPath = $"Shared.{CustomSqlNamingPattern.RepositoryClassName}";

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {CustomSqlNamingPattern.RepositoryClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {CustomSqlNamingPattern.RepositoryClassName}(ExecutionDataContext context) : base(context) {{}}");

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");

            var resultContractName     = CustomSqlNamingPattern.ResultClassName;
            var readContractMethodPath = $"{sharedRepositoryClassAccessPath}.ReadContract";

            if (CustomSqlInfo.ResultContractIsReferencedToEntity)
            {
                resultContractName     = CustomSqlNamingPattern.ReferencedEntityAccessPath;
                readContractMethodPath = CustomSqlNamingPattern.ReferencedEntityReaderMethodPath;

                RepositoryAssemblyReferences.Add(CustomSqlNamingPattern.ReferencedEntityAssemblyPath);
                RepositoryAssemblyReferences.Add(CustomSqlNamingPattern.ReferencedRepositoryAssemblyPath);
            }

            if (CustomSqlInfo.SqlResultIsCollection)
            {
                sb.AppendLine($"public GenericResponse<List<{resultContractName}>> Execute({CustomSqlNamingPattern.InputClassName} request)");
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
                sb.AppendLine($"public GenericResponse<{resultContractName}> Execute({CustomSqlNamingPattern.InputClassName} request)");
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

       
        #endregion
    }
}