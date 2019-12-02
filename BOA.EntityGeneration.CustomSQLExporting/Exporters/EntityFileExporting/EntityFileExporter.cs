using System.Linq;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.EntityFileExporting
{
    class EntityFileExporter : ContextContainer
    {
        static readonly EntityFileExporterConfig _config = EntityFileExporterConfig.CreateFromFile();

        #region Properties
        readonly PaddedStringBuilder sb =new PaddedStringBuilder();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoInitialized += UsingList;
            Context.ProfileInfoInitialized += BeginNamespace;

            Context.CustomSqlInfoInitialized += WriteSqlInputOutputTypes;

            Context.ProfileInfoRemove += EndNamespace;
            Context.ProfileInfoRemove += ExportFileToDirectory;
        }
        #endregion

        void EmptyLine()
        {
            sb.AppendLine();
        }
        #region Methods

        void UsingList()
        {
            foreach (var line in _config.UsingLines)
            {
                sb.AppendLine(Resolve(line));
            }
            
        }


        void BeginNamespace()
        {


            sb.AppendLine();
            sb.AppendLine($"namespace {ProfileNamingPattern.EntityNamespace}");
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
            ProcessInfo.Text = "Exporting Entity classes.";

            var filePath = ProfileNamingPattern.EntityProjectDirectory + "All.cs";

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

       

        void WriteSqlInputOutputTypes()
        {
            if (CustomSqlInfo.ResultColumns.Any(r => r.IsReferenceToEntity))
            {
                EntityAssemblyReferences.Add(CustomSqlNamingPattern.ReferencedEntityAssemblyPath);
            }

            var resultContractName = CustomSqlNamingPattern.ResultClassName;

            if (!CustomSqlInfo.ResultContractIsReferencedToEntity)
            {
                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///     Result class of '{CustomSqlInfo.Name}' sql.");
                sb.AppendLine("/// </summary>");
                sb.AppendLine("[Serializable]");
                sb.AppendLine($"public sealed class {resultContractName} : CardContractBase");
                sb.AppendLine("{");
                sb.PaddingCount++;

                foreach (var item in CustomSqlInfo.ResultColumns)
                {
                    sb.AppendLine($"public {item.DataTypeInDotnet} {item.NameInDotnet} {{get; set;}}");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            else
            {
                resultContractName = CustomSqlNamingPattern.ReferencedEntityAccessPath;
            }

            var interfaceName = $"ICustomSqlProxy<GenericResponse<{resultContractName}>, {resultContractName}>";
            if (CustomSqlInfo.SqlResultIsCollection)
            {
                interfaceName = $"ICustomSqlProxy<GenericResponse<List<{resultContractName}>>, List<{resultContractName}>>";
            }

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Parameter class of '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {CustomSqlNamingPattern.InputClassName} : {interfaceName}");

            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in CustomSqlInfo.Parameters)
            {
                sb.AppendLine($"public {item.CSharpPropertyTypeName} {item.CSharpPropertyName} {{ get; set; }}");
            }

            sb.AppendLine();
            sb.AppendLine($"int {interfaceName}.Index");
            sb.AppendLine("{");
            sb.AppendLine($"    get {{ return {CustomSqlInfo.SwitchCaseIndex}; }}");
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}