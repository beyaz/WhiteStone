using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.SharedRepositoryFileExporting.SharedFileExporter;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class SelectAllMethodWriter
    {
        #region Public Methods
        public static void Write(Context context)
        {
            var file      = File[context];
            var tableInfo = TableInfo[context];

            var sql = SelectAllInfoCreator.Create(tableInfo).Sql;

            file.AppendLine("public static SqlInfo Select()");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(sql);
            file.AppendLine();
            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("return new SqlInfo { CommandText = sql };");
            file.CloseBracket();
        }
        #endregion
    }
}