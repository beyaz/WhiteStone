using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.ScriptModel
{
    public class UpdateByPrimaryKeyInfo
    {
        #region Public Properties
        public string                    Sql                 { get; set; }
        public IReadOnlyList<IColumnInfo> SqlParameters       { get; set; }
        public IReadOnlyList<IColumnInfo> WhereParameters     { get; set; }
        public IReadOnlyList<IColumnInfo> ColumnsWillBeUpdate { get; set; }
        #endregion
    }
}