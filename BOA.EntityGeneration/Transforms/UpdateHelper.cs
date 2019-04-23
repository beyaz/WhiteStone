using System.Collections.Generic;
using System.Linq;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Transforms
{
    static class UpdateHelper
    {
        #region Public Methods
        public static IReadOnlyList<ColumnInfo> GetColumnsWillBeUpdate(this TableInfo tableInfo)
        {
            var excludedColumnNames = tableInfo.PrimaryKeyColumns.Select(x => x.ColumnName).ToList();

            excludedColumnNames.Add(Names.INSERT_DATE);
            excludedColumnNames.Add(Names.INSERT_TOKEN_ID);
            excludedColumnNames.Add(Names.INSERT_USER_ID);
            excludedColumnNames.Add(Names.ROW_GUID);
            excludedColumnNames.Add(Names.INSERT_USER_ID);

            return tableInfo.Columns.Where(c => !excludedColumnNames.Contains(c.ColumnName)).ToList();
        }

        public static IReadOnlyList<ColumnInfo> GetSqlInputParameters(this TableInfo tableInfo)
        {
            var parameters = new List<ColumnInfo>();

            parameters.AddRange(GetColumnsWillBeUpdate(tableInfo));
            parameters.AddRange(tableInfo.PrimaryKeyColumns);

            return parameters;
        }

        public static IReadOnlyList<ColumnInfo> GetWhereParameters(this TableInfo tableInfo)
        {
            var parameters = new List<ColumnInfo>();

            parameters.AddRange(tableInfo.PrimaryKeyColumns);

            return parameters;
        }
        #endregion
    }
}