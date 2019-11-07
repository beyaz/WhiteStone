using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters
{
    public class TypeClassWriter
    {
        public void Write_ICustomSqlProxy(PaddedStringBuilder sb)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Custom sql proxy.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("public interface ICustomSqlProxy<TOutput, T> where TOutput : GenericResponse<T>");
            sb.AppendLine("{");
            sb.AppendLine("    int Index { get; }");
            sb.AppendLine("}");
        }
        #region Public Methods
        public void Write(PaddedStringBuilder sb, ICustomSqlInfo data)
        {
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