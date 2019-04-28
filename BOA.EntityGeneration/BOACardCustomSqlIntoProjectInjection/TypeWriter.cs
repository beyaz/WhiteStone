using System;
using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    [Serializable]
    public class CustomSqlInfo
    {
        #region Public Properties
        public string Name { get; set; }

        public string                                ParameterContractName => Name.ToContractName() + "Request";
        public IReadOnlyList<CustomSqlInfoParameter> Parameters            { get; set; }
        public IReadOnlyList<CustomSqlInfoResult>    ResultColumns         { get; set; }

        public string ResultContractName => Name.ToContractName() + "Contract";
        public string Sql                { get; set; }
        #endregion
    }

    [Serializable]
    public class CustomSqlInfoParameter
    {
        #region Public Properties
        public string DataType         { get; set; }
        public string DataTypeInDotnet { get; set; }
        public string Name             { get; set; }

        public string NameInDotnet { get; set; }
        #endregion
    }

    [Serializable]
    public class CustomSqlInfoResult
    {
        #region Public Properties
        public string DataType { get; set; }

        public string DataTypeInDotnet { get; set; }
        public string Name             { get; set; }

        public string NameInDotnet { get; set; }
        #endregion
    }

    public class TypeWriter
    {
        #region Public Methods
        public void Write(PaddedStringBuilder sb, CustomSqlInfo data)
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

            sb.AppendLine("}");
            sb.PaddingCount--;

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Parameter class of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {data.ParameterContractName} : CardContractBase");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.Parameters)
            {
                sb.AppendLine($"public {item.DataTypeInDotnet} {item.NameInDotnet} {{get; set;}}");
            }

            sb.AppendLine("}");
            sb.PaddingCount--;
        }
        #endregion
    }
}