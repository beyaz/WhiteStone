﻿using System.IO;
using BOA.Common.Helpers;
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

            FileSystem.WriteAllText(filePath, sb.ToString());
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

       
        #endregion
    }
}