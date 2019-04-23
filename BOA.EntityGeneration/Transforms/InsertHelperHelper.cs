using System.Collections.Generic;
using System.Linq;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Transforms
{
    static class InsertHelperHelper
    {
        #region Public Methods
        public static IReadOnlyList<ColumnInfo> GetColumnsWillBeInsert(this TableInfo tableInfo)
        {
            var excludedColumnNames = new List<string>
            {
                Names.UPDATE_DATE,
                Names.UPDATE_USER_ID,
                Names.UPDATE_TOKEN_ID
            };

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }
        #endregion
    }
}