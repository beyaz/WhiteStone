using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using BOA.EntityGeneration.Util;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.SharedRepositoryFileExporting.SharedFileExporter;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class InsertMethodWriter
    {
        #region Public Methods
        public static void Write(IContext context)
        {
            var file             = File[context];
            var tableInfo        = TableInfo[context];
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles[context];

            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            var sqlParameters = insertInfo.SqlParameters;

            file.AppendLine($"public static SqlInfo Insert({typeContractName} contract)");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(insertInfo.Sql);
            file.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                file.AppendLine("SELECT CAST(SCOPE_IDENTITY() AS INT)");
            }

            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                file.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
                }
            }

            file.AppendLine();
            file.AppendLine("return sqlInfo;");

            file.CloseBracket();
        }
        #endregion
    }
}