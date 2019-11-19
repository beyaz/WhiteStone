using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.CustomSqlNamingPatternContract;
using static BOA.EntityGeneration.CustomSQLExporting.ProfileNamingPatternContract;
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
            context.AttachEvent(OnProfileInfoInitialized, CustomSqlClassGenerator.InitializeText);
            
            CustomSqlClassGenerator.AttachEvents(context);

            context.AttachEvent(OnProfileInfoInitialized, InitializeOutput);
            context.AttachEvent(OnProfileInfoInitialized, UsingList);
            context.AttachEvent(OnProfileInfoInitialized, EmptyLine);
            context.AttachEvent(OnProfileInfoInitialized, BeginNamespace);
            context.AttachEvent(OnProfileInfoInitialized, WriteEmbeddedClasses);

            context.AttachEvent(OnCustomSqlInfoInitialized, WriteBoaRepositoryClass);

            context.AttachEvent(OnProfileInfoRemove, WriteProxyClass);
            context.AttachEvent(OnProfileInfoRemove, EndNamespace);
            context.AttachEvent(OnProfileInfoRemove, ExportFileToDirectory);

            


            
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb            = context.Get(File);
            var profileNamingPattern= context.Get(ProfileNamingPattern);

            sb.BeginNamespace(profileNamingPattern.RepositoryNamespace);
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
            var profileNamingPattern = context.Get(ProfileNamingPattern);

            var processInfo = context.Get(ProcessInfo);

            processInfo.Text = "Exporting BOA repository.";

            var filePath = profileNamingPattern.RepositoryProjectDirectory + "Boa.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void UsingList(IDataContext context)
        {
            var sb            = context.Get(File);
            var profileNamingPattern = context.Get(ProfileNamingPattern);

            foreach (var line in profileNamingPattern.BoaRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }

        static void WriteBoaRepositoryClass(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(ProfileNamingPattern);
            var customSqlInfo          = context.Get(CustomSqlInfo);
            var customSqlNamingPattern = context.Get(CustomSqlNamingPattern);

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

            var resultContractName = customSqlNamingPattern.ResultClassName;
            var readContractMethodPath = $"{sharedRepositoryClassAccessPath}.ReadContract";

            if (customSqlInfo.ResultContractIsReferencedToEntity)
            {
                resultContractName = customSqlNamingPattern.ReferencedEntityAccessPath;
                
                readContractMethodPath = customSqlInfo.SchemaName + ".Shared." + customSqlInfo.ResultColumns[0].Name.ToContractName() + ".ReadContract";
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

            var proxyClass = context.Get(CustomSqlClassGenerator.Text).ToString();
            sb.AppendAll(proxyClass);
            sb.AppendLine();
        }
        #endregion
    }

    class CustomSqlClassGenerator
    {
        public static readonly IDataConstant<PaddedStringBuilder> Text = DataConstant.Create<PaddedStringBuilder>(nameof(CustomSqlClassGenerator) + "->" + nameof(Text));

        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, Begin);

            context.AttachEvent(OnCustomSqlInfoInitialized, WriteSwitchCaseCondition);

            context.AttachEvent(OnProfileInfoRemove, End);
        }


        static void WriteSwitchCaseCondition(IDataContext context)
        {
            var sb                     = context.Get(Text);
            var customSqlInfo = context.Get(CustomSqlInfo);
            var customSqlNamingPattern = context.Get(CustomSqlNamingPattern);

            sb.AppendLine($"case {customSqlInfo.SwitchCaseIndex}:");
            sb.OpenBracket();
            sb.AppendLine($"return (TOutput) (object) new {customSqlNamingPattern.RepositoryClassName}(objectHelper.Context).Execute(({customSqlNamingPattern.InputClassName})(object) input);");
            sb.CloseBracket();

           
        }

        public static void InitializeText(IDataContext context)
        {
            context.Add(Text,new PaddedStringBuilder());
        }
        
        static void Begin(IDataContext context)
        {
            

            var sb                     = context.Get(Text);

            sb.AppendLine("public static class CustomSql");
            sb.OpenBracket();

            sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            sb.OpenBracket();

            sb.AppendLine("switch (input.Index)");
            sb.OpenBracket();

           
        }

        static void End(IDataContext context)
        {
            var sb                     = context.Get(Text);
            

           

            sb.CloseBracket(); // end of switch

            sb.AppendLine();
            sb.AppendLine("throw new System.InvalidOperationException(input.GetType().FullName);");

            sb.CloseBracket(); // end of method

            sb.CloseBracket(); // end of class
        }
    }
}