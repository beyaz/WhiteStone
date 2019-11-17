using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared
{
    static class SelectAllByValidFlagMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(SharedFileExporter.File);
            var tableInfo = context.Get(Data.TableInfo);
            var sql       = SelectAllInfoCreator.Create(tableInfo).Sql;

            sb.AppendLine($"public static SqlInfo SelectByValidFlag()");
            sb.OpenBracket();

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(sql + " WHERE [VALID_FLAG] = '1'");
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();

            sb.AppendLine("return new SqlInfo { CommandText = sql };");
            sb.CloseBracket();
        }
        #endregion
    }
}