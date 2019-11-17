using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public static class BoaRepositoryFileExporter
    {
        #region Static Fields
        public static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(BoaRepositoryFileExporter) + "->" + nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeOutput);
            context.AttachEvent(OnProfileInfoInitialized, UsingList);
            context.AttachEvent(OnProfileInfoInitialized, EmptyLine);
            context.AttachEvent(OnProfileInfoInitialized, BeginNamespace);
            context.AttachEvent(OnProfileInfoInitialized, WriteEmbeddedClasses);

            context.AttachEvent(OnCustomSqlInfoInitialized, WriteBoaRepositoryClass);

            context.AttachEvent(OnProfileInfoRemove, WriteProxyClass);
            context.AttachEvent(OnProfileInfoRemove, EndNamespace);
            context.AttachEvent(OnProfileInfoRemove, ExportFileToDirectory);
            context.AttachEvent(OnProfileInfoRemove, ClearOutput);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(NamingPattern.Id);

            sb.BeginNamespace(namingPattern.RepositoryNamespace);
        }

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.CloseBracket();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(NamingPattern.Id);

            var processInfo = context.Get(ProcessInfo);

            processInfo.Text = "Exporting BOA repository.";

            var filePath = namingPattern.RepositoryProjectDirectory + "Boa.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void UsingList(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(NamingPattern.Id);

            foreach (var line in namingPattern.BoaRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }

        static void WriteBoaRepositoryClass(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(NamingPattern.Id);
            var data          = context.Get(CustomSqlInfo);

            var key = $"{namingPattern.RepositoryNamespace}.{data.BusinessClassName}.Execute";

            var sharedRepositoryClassAccessPath = $"Shared.{data.BusinessClassName}";

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {data.BusinessClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {data.BusinessClassName}(ExecutionDataContext context) : base(context) {{}}");

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            if (data.SqlResultIsCollection)
            {
                sb.AppendLine($"public GenericResponse<List<{data.ResultContractName}>> Execute({data.ParameterContractName} request)");
                sb.OpenBracket();
                sb.AppendLine($"const string CallerMemberPath = \"{key}\";");
                sb.AppendLine();
                sb.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.CreateSqlInfo(request);");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderToList<{data.ResultContractName}>(CallerMemberPath, sqlInfo, {sharedRepositoryClassAccessPath}.ReadContract);");
                sb.CloseBracket();
            }
            else
            {
                sb.AppendLine($"public GenericResponse<{data.ResultContractName}> Execute({data.ParameterContractName} request)");
                sb.OpenBracket();
                sb.AppendLine($"const string CallerMemberPath = \"{key}\";");
                sb.AppendLine();
                sb.AppendLine($"var sqlInfo = {sharedRepositoryClassAccessPath}.CreateSqlInfo(request);");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderToContract<{data.ResultContractName}>(CallerMemberPath, sqlInfo, {sharedRepositoryClassAccessPath}.ReadContract);");

                sb.CloseBracket();
            }

            sb.AppendLine();

            sb.CloseBracket();
        }

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb = context.Get(File);

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        static void WriteProxyClass(IDataContext context)
        {
            var sb             = context.Get(File);
            var customSqlInfos = context.Get(ProcessedCustomSqlInfoListInProfile);

            sb.AppendLine("public static class CustomSql");
            sb.OpenBracket();

            sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            sb.OpenBracket();

            sb.AppendLine("switch (input.Index)");
            sb.OpenBracket();

            foreach (var customSqlInfo in customSqlInfos)
            {
                sb.AppendLine($"case {customSqlInfo.SwitchCaseIndex}:");
                sb.OpenBracket();
                sb.AppendLine($"return (TOutput) (object) new {customSqlInfo.BusinessClassName}(objectHelper.Context).Execute(({customSqlInfo.ParameterContractName})(object) input);");
                sb.CloseBracket();
            }

            sb.CloseBracket(); // end of switch

            sb.AppendLine();
            sb.AppendLine("throw new System.InvalidOperationException(input.GetType().FullName);");

            sb.CloseBracket(); // end of method

            sb.CloseBracket(); // end of class
        }
        #endregion
    }
}