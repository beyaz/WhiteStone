using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;
using static BOA.EntityGeneration.CustomSQLExporting.Exporters.EntityCsprojFileExporter;
    
namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    static class EntityFileExporter
    {
        #region Static Fields
        
        static readonly        IDataConstant<PaddedStringBuilder> File               = DataConstant.Create<PaddedStringBuilder>(nameof(EntityFileExporter) + "->" + nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeOutput);
            context.AttachEvent(OnProfileInfoInitialized, BeginNamespace);

            context.AttachEvent(OnCustomSqlInfoInitialized, WriteSqlInputOutputTypes);

            context.AttachEvent(OnProfileInfoRemove, EndNamespace);
            context.AttachEvent(OnProfileInfoRemove, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb                   = context.Get(File);
            var profileNamingPattern = context.Get(Data.ProfileNamingPattern);

            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {profileNamingPattern.EntityNamespace}");
            sb.OpenBracket();

            sb.AppendLine("/// <summary>");
            sb.AppendLine("///     Custom sql proxy.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("public interface ICustomSqlProxy<TOutput, T> where TOutput : GenericResponse<T>");
            sb.AppendLine("{");
            sb.AppendLine("    int Index { get; }");
            sb.AppendLine("}");
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.CloseBracket();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb                   = context.Get(File);
            var profileNamingPattern = context.Get(Data.ProfileNamingPattern);

            var processInfo = context.Get(ProcessInfo);

            processInfo.Text = "Exporting Entity classes.";

            var filePath = profileNamingPattern.EntityProjectDirectory + "All.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            File[context]               = new PaddedStringBuilder();
           
        }

        static void WriteSqlInputOutputTypes(IDataContext context)
        {
            var sb                     = context.Get(File);
            var customSqlInfo          = context.Get(CustomSqlInfo);
            var customSqlNamingPattern = context.Get(Data.CustomSqlNamingPattern);

            if (customSqlInfo.ResultColumns.Any(r => r.IsReferenceToEntity))
            {
                Data.EntityAssemblyReferences[context].Add(customSqlNamingPattern.ReferencedEntityAssemblyPath);
            }

            var resultContractName = customSqlNamingPattern.ResultClassName;

            if (!customSqlInfo.ResultContractIsReferencedToEntity)
            {
                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///     Result class of '{customSqlInfo.Name}' sql.");
                sb.AppendLine("/// </summary>");
                sb.AppendLine("[Serializable]");
                sb.AppendLine($"public sealed class {resultContractName} : CardContractBase");
                sb.AppendLine("{");
                sb.PaddingCount++;

                foreach (var item in customSqlInfo.ResultColumns)
                {
                    sb.AppendLine($"public {item.DataTypeInDotnet} {item.NameInDotnet} {{get; set;}}");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            else
            {
                resultContractName = customSqlNamingPattern.ReferencedEntityAccessPath;
            }

            var interfaceName = $"ICustomSqlProxy<GenericResponse<{resultContractName}>, {resultContractName}>";
            if (customSqlInfo.SqlResultIsCollection)
            {
                interfaceName = $"ICustomSqlProxy<GenericResponse<List<{resultContractName}>>, List<{resultContractName}>>";
            }

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Parameter class of '{customSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {customSqlNamingPattern.InputClassName} : {interfaceName}");

            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in customSqlInfo.Parameters)
            {
                sb.AppendLine($"public {item.CSharpPropertyTypeName} {item.CSharpPropertyName} {{ get; set; }}");
            }

            sb.AppendLine();
            sb.AppendLine($"int {interfaceName}.Index");
            sb.AppendLine("{");
            sb.AppendLine($"    get {{ return {customSqlInfo.SwitchCaseIndex}; }}");
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}