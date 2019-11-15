using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    public static class TypeFileExporter
    {
        #region Static Fields
        public static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(CustomSqlExporter.ProfileInfoIsAvailable, InitializeOutput);
            context.AttachEvent(CustomSqlExporter.ProfileInfoIsAvailable, BeginNamespace);

            context.AttachEvent(CustomSqlExporter.CustomSqlInfoIsAvailable, WriteSqlInputOutputTypes);

            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, EndNamespace);
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, ExportFileToDirectory);
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, ClearOutput);
        }
        #endregion

        #region Methods
        static void ExportFileToDirectory(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlExporter.CustomSqlInfoProject);
            var fileAccess = context.Get(Data.FileAccess);

            
            fileAccess.WriteAllText(data.TypesProjectPath + "\\Generated\\CustomSql.cs", sb.ToString());
        }

        static void BeginNamespace(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlExporter.CustomSqlInfoProject);

            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {data.NamespaceNameOfType}");
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

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }
        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void WriteSqlInputOutputTypes(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlExporter.CustomSqlInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Result class of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {data.ResultContractName} : CardContractBase");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.ResultColumns)
            {
                sb.AppendLine($"public {item.DataTypeInDotnet} {item.NameInDotnet} {{get; set;}}");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            var interfaceName = $"ICustomSqlProxy<GenericResponse<{data.ResultContractName}>, {data.ResultContractName}>";
            if (data.SqlResultIsCollection)
            {
                interfaceName = $"ICustomSqlProxy<GenericResponse<List<{data.ResultContractName}>>, List<{data.ResultContractName}>>";
            }

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Parameter class of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");

            //if (data.ProfileId == "CC_OPERATIONS" ||
            //    data.ProfileId == "ACQUIRING_KERNEL"||
            //    data.ProfileId == "CC_OPERATIONS_KERNEL"||
            //    data.ProfileId == "CreditCardExtract"||
            //    data.ProfileId == "ACCOUNTING"||
            //    data.ProfileId == "EMC_MANAGEMENT")
            //{
            sb.AppendLine($"public sealed class {data.ParameterContractName} : {interfaceName}");
            //}
            //else
            //{
            //    sb.AppendLine($"public partial class {data.ParameterContractName} : CardRequestBase, {interfaceName}");
            //}

            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.Parameters)
            {
                sb.AppendLine($"public {item.CSharpPropertyTypeName} {item.CSharpPropertyName} {{ get; set; }}");
            }

            sb.AppendLine();
            sb.AppendLine($"int {interfaceName}.Index");
            sb.AppendLine("{");
            sb.AppendLine($"    get {{ return {data.SwitchCaseIndex}; }}");
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}