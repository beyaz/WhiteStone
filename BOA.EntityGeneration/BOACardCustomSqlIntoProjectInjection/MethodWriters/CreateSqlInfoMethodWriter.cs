using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.MethodWriters
{
    static class CreateSqlInfoMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, ICustomSqlInfo data)
        {
            sb.AppendLine($"static SqlInfo CreateSqlInfo({data.ParameterContractName} request)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("var sqlInfo = new SqlInfo();");
            if (data.Parameters.Any())
            {
                sb.AppendLine();
                foreach (var item in data.Parameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{item.Name}\", SqlDbType.{item.SqlDbTypeName}, request.{item.ValueAccessPathForAddInParameter});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}