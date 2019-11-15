using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.MethodWriters
{
    static class ReadContractMethodWriter
    {
        #region Public Methods
        public static void WriteReadContract(PaddedStringBuilder sb, ICustomSqlInfo data)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Maps reader columns to contract for '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataRecord reader, {data.ResultContractName} contract)");
            sb.OpenBracket();

            foreach (var item in data.ResultColumns)
            {
                sb.AppendLine($"contract.{item.NameInDotnet} = SQLDBHelper.{item.SqlReaderMethod}(reader[\"{item.Name}\"]);");
            }

            sb.CloseBracket();
        }
        #endregion
    }
}