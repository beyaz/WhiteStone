using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Models
{
    class AllSchemaInOneClassRepositoryNamingPatternContract
    {
        #region Public Properties
        public IReadOnlyList<string> UsingLines    { get; set; }
        public IReadOnlyList<string> ExtraAssemblyReferences { get; set; }
        
        public string                NamespaceName { get; set; }
        public string ClassName { get; set; }
        #endregion
    }
}