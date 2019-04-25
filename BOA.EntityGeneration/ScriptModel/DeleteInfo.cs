using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.ScriptModel
{

    [Serializable]
    public class DeleteInfo
    {
        #region Public Properties
        public string                    Sql           { get; set; }
        public IReadOnlyList<ColumnInfo> SqlParameters { get; set; }
        #endregion
    }


    [Serializable]
    public class ContractBodyDbMembers
    {
        #region Public Properties
        public string                    PropertyDefinitions           { get; set; }
        #endregion
    }
}