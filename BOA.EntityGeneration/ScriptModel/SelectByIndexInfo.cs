using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.ScriptModel
{
    [Serializable]
    public class SelectByIndexInfo
    {
        #region Public Properties
        public string                    Sql           { get; set; }
        public IReadOnlyList<IColumnInfo> SqlParameters { get; set; }
        #endregion
    }



}