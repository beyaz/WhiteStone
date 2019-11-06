using System.Collections.Generic;
using System.Linq;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class InsertInfoCreator : ScriptModel.Creators.InsertInfoCreator
    {
        #region Methods
        protected override IReadOnlyList<IColumnInfo> GetColumnsWillBeInsert(ITableInfo tableInfo)
        {
            var excludedColumnNames = new List<string>
            {
                Names2.UPDATE_DATE,
                Names2.UPDATE_USER_ID,
                Names2.UPDATE_TOKEN_ID,

                Names2.UpdateUserName,
                Names2.UpdateHostName,
                Names2.UpdateHostIP,
                Names2.UpdateSystemDate
            };

            if (tableInfo.HasIdentityColumn)
            {
                excludedColumnNames.Add(tableInfo.IdentityColumn.ColumnName);
            }

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }
        #endregion
    }
}