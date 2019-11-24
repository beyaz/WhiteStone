using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityFileExporter : ContextContainer
    {
        #region Static Fields
        static readonly Property<PaddedStringBuilder> File = Property.Create<PaddedStringBuilder>(nameof(EntityFileExporter) + "->" + nameof(File));
        #endregion

        #region Properties
        PaddedStringBuilder sb => File[Context];
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoInitialized += InitializeOutput;
            Context.ProfileInfoInitialized += BeginNamespace;

            Context.CustomSqlInfoInitialized += WriteSqlInputOutputTypes;

            Context.ProfileInfoRemove += EndNamespace;
            Context.ProfileInfoRemove += ExportFileToDirectory;
        }
        #endregion

        #region Methods
        void BeginNamespace()
        {
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

        void EndNamespace()
        {
            sb.CloseBracket();
        }

        void ExportFileToDirectory()
        {
            processInfo.Text = "Exporting Entity classes.";

            var filePath = profileNamingPattern.EntityProjectDirectory + "All.cs";

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

        void InitializeOutput()
        {
            File[Context] = new PaddedStringBuilder();
        }

        void WriteSqlInputOutputTypes()
        {
            if (customSqlInfo.ResultColumns.Any(r => r.IsReferenceToEntity))
            {
                entityAssemblyReferences.Add(customSqlNamingPattern.ReferencedEntityAssemblyPath);
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