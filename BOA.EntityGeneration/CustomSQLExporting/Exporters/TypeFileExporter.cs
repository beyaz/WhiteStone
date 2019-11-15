using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public static class TypeFileExporter
    {
        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(TypeFileExporter) + "->" + nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(ProfileInfoIsAvailable, InitializeOutput);
            context.AttachEvent(ProfileInfoIsAvailable, BeginNamespace);
            context.AttachEvent(ProfileInfoIsAvailable, EndNamespace);
            context.AttachEvent(ProfileInfoIsAvailable, ExportFileToDirectory);
            context.AttachEvent(ProfileInfoIsAvailable, ClearOutput);

            context.AttachEvent(CustomSqlInfoIsAvailable, WriteSqlInputOutputTypes);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlProfileInfo);

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

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.CloseBracket();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb         = context.Get(File);
            var data       = context.Get(CustomSqlProfileInfo);
            var fileAccess = context.Get(Data.FileAccess);

            var config = context.Get(Data.Config);

            var filePath = config.CustomSQLOutputFilePathForEntity.Replace("{ProfileId}", context.Get(ProfileId));

            fileAccess.WriteAllText(filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteSqlInputOutputTypes(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlInfo);

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