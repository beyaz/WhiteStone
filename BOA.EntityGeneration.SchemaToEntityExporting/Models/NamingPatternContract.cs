using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Models
{

    static class NamingMapKey
    {
        public const string EntityNamespaceName = "$(EntityNamespace)";
        public const string EntityClassName = "$(EntityClassName)";
        public const string CamelCasedTableName = "$(CamelCasedTableName)";
    }

    class NamingPatternContract
    {
        #region Public Properties
      
        public string                EntityProjectDirectory       { get; set; }
        public IReadOnlyList<string> RepositoryAssemblyReferences { get; set; }
        public string                RepositoryNamespace          { get; set; }
        public string                RepositoryProjectDirectory   { get; set; }
        public string                SlnDirectoryPath             { get; set; }
        #endregion
    }
}