using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Generators
{
    static class ParameterHelper
    {
        #region Public Methods
        public static string ConvertToParameterDeclarationCode(this ColumnInfo columnInfo)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("new Parameter");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"Name = \"@{columnInfo.ColumnName}\",");
            sb.AppendLine($"SqlDbType = SqlDbType.{columnInfo.SqlDatabaseTypeName},");

            if (columnInfo.ColumnName == Names.ROW_GUID)
            {
                sb.AppendLine($"Value = {@"Guid.NewGuid().ToString().ToUpper(new System.Globalization.CultureInfo(""en-US"", false))"}");
            }
            else if (columnInfo.ColumnName == Names.VALID_FLAG)
            {
                sb.AppendLine($"Value = {columnInfo.ColumnName.ToContractName()} ? \"1\" : \"0\"");
            }

            else if (columnInfo.ColumnName == Names.INSERT_DATE || columnInfo.ColumnName == Names.UPDATE_DATE)
            {
                sb.AppendLine("Value = DateTime.Now");
            }
            else if (columnInfo.ColumnName == Names.INSERT_USER_ID || columnInfo.ColumnName == Names.UPDATE_USER_ID)
            {
                sb.AppendLine("Value = context.AuthenticationUserName");
            }
            else if (columnInfo.ColumnName == Names.INSERT_TOKEN_ID || columnInfo.ColumnName == Names.UPDATE_TOKEN_ID)
            {
                sb.AppendLine("Value = Convert.ToString(context.EngineContextMainBusinessKey)");
            }

            else
            {
                sb.AppendLine($"Value = {columnInfo.ColumnName.ToContractName()}");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        public static string GetValueForSqlInsert(ColumnInfo columnInfo,string contractVariableName = "contract")
        {
            if (columnInfo.ColumnName == Names.ROW_GUID)
            {
                return $"{@"Guid.NewGuid().ToString().ToUpper(new System.Globalization.CultureInfo(""en-US"", false))"}";
            }

            if (columnInfo.ColumnName == Names.VALID_FLAG)
            {
                return $"{contractVariableName}.{columnInfo.ColumnName.ToContractName()} ? \"1\" : \"0\"";
            }

            if (columnInfo.ColumnName == Names.INSERT_DATE || columnInfo.ColumnName == Names.UPDATE_DATE)
            {
                return "DateTime.Now";
            }

            if (columnInfo.ColumnName == Names.INSERT_USER_ID || columnInfo.ColumnName == Names.UPDATE_USER_ID)
            {
                return "Context.ApplicationContext.Authentication.UserName";
            }

            if (columnInfo.ColumnName == Names.INSERT_TOKEN_ID || columnInfo.ColumnName == Names.UPDATE_TOKEN_ID)
            {
                return "Convert.ToString(Context.EngineContext.MainBusinessKey)";
            }

            return $"{contractVariableName}.{columnInfo.ColumnName.ToContractName()}";
        }

        
        public static string GetValueForSqlUpdate(ColumnInfo columnInfo, string contractVariableName = "contract")
        {
            

            if (columnInfo.ColumnName == Names.VALID_FLAG)
            {
                return $"{contractVariableName}.{columnInfo.ColumnName.ToContractName()} ? \"1\" : \"0\"";
            }

            if (columnInfo.ColumnName == Names.INSERT_DATE || columnInfo.ColumnName == Names.UPDATE_DATE)
            {
                return "DateTime.Now";
            }

            if (columnInfo.ColumnName == Names.INSERT_USER_ID || columnInfo.ColumnName == Names.UPDATE_USER_ID)
            {
                return "Context.ApplicationContext.Authentication.UserName";
            }

            if (columnInfo.ColumnName == Names.INSERT_TOKEN_ID || columnInfo.ColumnName == Names.UPDATE_TOKEN_ID)
            {
                return "Convert.ToString(Context.EngineContext.MainBusinessKey)";
            }

            return $"{contractVariableName}.{columnInfo.ColumnName.ToContractName()}";
        }

        #endregion
    }
}