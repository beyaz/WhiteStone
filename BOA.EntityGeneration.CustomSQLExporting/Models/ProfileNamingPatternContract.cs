using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Models
{
    class ProfileNamingPatternContract
    {
        #region Public Properties
        public string                EntityNamespace              { get; set; }
        public string                EntityProjectDirectory       { get; set; }
        public string                RepositoryNamespace          { get; set; }
        public string                RepositoryProjectDirectory   { get; set; }
        public string                SlnDirectoryPath             { get; set; }
        #endregion
    }
}