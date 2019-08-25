using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration
{
    [Serializable]
    public class Config
    {
        #region Public Properties
        public bool                        BuildAfterCodeGenerationIsCompleted { get; set; }
        public string                      ConnectionString                    { get; set; }
        public string                      DaoAssemblyNamespaceFormat          { get; set; }
        public string                      DatabaseEnumName                    { get; set; }
        public string                      EntityAssemblyNamespaceFormat       { get; set; }
        public bool                        IntegrateWithBOATfs                 { get; set; }
        public IReadOnlyCollection<string> NotExportableTables                 { get; set; }
        public IReadOnlyCollection<string> SchemaNamesToBeExport               { get; set; }
        public string                      SlnDirectoryPath                    { get; set; }
        public string                      SqlSequenceInformationOfTable       { get; set; }
        public string                      TableCatalog                        { get; set; }

        public string                      TypeContractBase { get; set; }
        public IReadOnlyCollection<string> TypeUsingLines   { get; set; }
        public string FilePathForAllEntitiesInOneFile { get; set; }
        public bool EnableFullProjectExport { get; set; }
        public string FilePathForAllDaoInOneFile { get; set; }
        
        #endregion
    }
}