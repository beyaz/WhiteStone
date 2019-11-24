using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Models
{
    class ProfileNamingPatternContract
    {
        #region Public Properties
        public IReadOnlyList<string> BoaRepositoryUsingLines      { get; set; }
        public List<string>          EntityAssemblyReferences     { get; set; }
        public string                EntityNamespace              { get; set; }
        public string                EntityProjectDirectory       { get; set; }
        public IReadOnlyList<string> EntityUsingLines             { get; set; }
        public List<string>          RepositoryAssemblyReferences { get; set; }
        public string                RepositoryNamespace          { get; set; }
        public string                RepositoryProjectDirectory   { get; set; }
        public string                SlnDirectoryPath             { get; set; }
        #endregion
    }
}