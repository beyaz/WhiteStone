using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using BOA.EntityGeneration.Util;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class UpdateByPrimaryKeyMethodWriter
    {
        const string contractParameterName = "contract";

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb                     = context.Get(SharedFileExporter.File);
            var tableInfo              = context.Get(TableInfo);
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);

            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            var sqlParameters = updateInfo.SqlParameters;

            sb.AppendLine($"public static SqlInfo Update({typeContractName} {contractParameterName})");
            sb.OpenBracket();

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(updateInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();

            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlUpdate(columnInfo)});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.CloseBracket();
        }
        #endregion
    }
}