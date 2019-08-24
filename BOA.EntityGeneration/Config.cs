using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration
{
    [Serializable]
    public class Config
    {
        #region Public Properties
        public IReadOnlyCollection<string> NotExportableTables   { get; set; }
        public IReadOnlyCollection<string> SchemaNamesToBeExport { get; set; }
        public string ConnectionString { get; set; }
        #endregion
    }
}