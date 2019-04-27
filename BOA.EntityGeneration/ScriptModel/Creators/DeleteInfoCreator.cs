using System.Linq;
using BOA.EntityGeneration.DbModel.Types;
using BOA.EntityGeneration.ScriptModel.Types;

namespace BOA.EntityGeneration.ScriptModel.Creators
{
    public static class DeleteInfoCreator
    {
        #region Public Methods
        public static DeleteInfo Create(TableInfo tableInfo)
        {
            var parameters = tableInfo.PrimaryKeyColumns;

            return new DeleteInfo
            {
                Sql           = $"DELETE FROM [{tableInfo.SchemaName}].[{tableInfo.TableName}] WHERE {string.Join(" , ", parameters.Select(c => $"[{c.ColumnName}] = @{c.ColumnName}"))}",
                SqlParameters = parameters
            };
        }
        #endregion
    }
}