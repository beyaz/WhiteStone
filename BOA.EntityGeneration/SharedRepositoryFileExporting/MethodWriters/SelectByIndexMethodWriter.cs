using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.SharedRepositoryFileExporting.SharedFileExporter;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class SelectByIndexMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var file      = File[context];
            var tableInfo = TableInfo[context];

            var allIndexes = new List<IIndexInfo>();
            allIndexes.AddRange(tableInfo.NonUniqueIndexInfoList);
            allIndexes.AddRange(tableInfo.UniqueIndexInfoList);

            foreach (var indexIdentifier in allIndexes)
            {
                var indexInfo     = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);
                var methodName    = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));
                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public static SqlInfo {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine("const string sql = @\"");
                file.AppendAll(indexInfo.Sql);
                file.AppendLine();
                file.AppendLine("\";");
                file.AppendLine();
                file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

                if (indexInfo.SqlParameters.Any())
                {
                    file.AppendLine();
                    foreach (var columnInfo in indexInfo.SqlParameters)
                    {
                        file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                    }
                }

                file.AppendLine();
                file.AppendLine("return sqlInfo;");

                file.CloseBracket();
            }
        }
        #endregion
    }
}