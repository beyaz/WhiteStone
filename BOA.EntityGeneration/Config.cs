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
        public string TableCatalog { get; set; }
        public string SqlSequenceInformationOfTable { get; set; }
        public string EntityAssemblyNamespaceFormat { get; set; }
        public string DaoAssemblyNamespaceFormat { get; set; }
        public string SlnDirectoryPath { get; set; }
        public bool IntegrateWithBOATfs { get; set; }
        public string DatabaseEnumName { get; set; }
        public bool BuildAfterCodeGenerationIsCompleted { get; set; }
        #endregion
    }
}