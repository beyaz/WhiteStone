using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared
{
    static class SelectAllMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(Data.SharedRepositoryFile);
            var tableInfo = context.Get(Data.TableInfo);
            var sql       = SelectAllInfoCreator.Create(tableInfo).Sql;

            sb.AppendLine($"public static SqlInfo Select()");
            sb.OpenBracket();

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();

            sb.AppendLine("return new SqlInfo { CommandText = sql };");
            sb.CloseBracket();
        }
        #endregion
    }
}