using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{
    class NamingPatternContract
    {
        #region Public Properties
        public IReadOnlyList<string> UsingLines              { get; set; }
        public IReadOnlyList<string> ExtraAssemblyReferences { get; set; }
        
        public string NamespaceName { get; set; }
        public string ClassName     { get; set; }
        #endregion
    }
}