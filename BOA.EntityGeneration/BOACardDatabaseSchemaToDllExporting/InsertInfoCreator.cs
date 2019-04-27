using System.Collections.Generic;
using System.Linq;
using BOA.EntityGeneration.DbModel;
using static BOA.EntityGeneration.Names2;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class InsertInfoCreator : ScriptModel.Creators.InsertInfoCreator
    {
        #region Methods
        protected override IReadOnlyList<ColumnInfo> GetColumnsWillBeInsert(DbModel.TableInfo tableInfo)
        {
            var excludedColumnNames = new List<string>
            {
                UPDATE_DATE,
                UPDATE_USER_ID,
                UPDATE_TOKEN_ID,

                UpdateUserName,
                UpdateHostName,
                UpdateHostIP,
                UpdateSystemDate
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