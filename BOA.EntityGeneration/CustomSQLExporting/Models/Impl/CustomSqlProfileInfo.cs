using System;
using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Impl
{
    [Serializable]
    public class CustomSqlProfileInfo : ICustomSqlProfileInfo
    {
        #region Public Properties
        public IReadOnlyList<string> ObjectIdList { get; set; }
        #endregion
    }
}