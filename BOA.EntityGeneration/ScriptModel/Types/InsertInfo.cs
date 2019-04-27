using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.ScriptModel
{
    [Serializable]
    public class InsertInfo
    {
        #region Public Properties
        public string                    Sql           { get; set; }
        public IReadOnlyList<ColumnInfo> SqlParameters { get; set; }
        #endregion
    }
}