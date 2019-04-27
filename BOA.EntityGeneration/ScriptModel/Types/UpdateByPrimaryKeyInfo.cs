using System.Collections.Generic;
using BOA.EntityGeneration.DbModel.Types;

namespace BOA.EntityGeneration.ScriptModel.Types
{
    public class UpdateByPrimaryKeyInfo
    {
        #region Public Properties
        public string                    Sql                 { get; set; }
        public IReadOnlyList<ColumnInfo> SqlParameters       { get; set; }
        public IReadOnlyList<ColumnInfo> WhereParameters     { get; set; }
        public IReadOnlyList<ColumnInfo> ColumnsWillBeUpdate { get; set; }
        #endregion
    }
}