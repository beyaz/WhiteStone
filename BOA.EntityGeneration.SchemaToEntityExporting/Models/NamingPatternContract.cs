using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Models
{
    class NamingPatternContract
    {
        #region Public Properties
      
        public string                EntityNamespace              { get; set; }
        public string                EntityProjectDirectory       { get; set; }
        public IReadOnlyList<string> RepositoryAssemblyReferences { get; set; }
        public string                RepositoryNamespace          { get; set; }
        public string                RepositoryProjectDirectory   { get; set; }
        public string                SlnDirectoryPath             { get; set; }
        #endregion
    }
}