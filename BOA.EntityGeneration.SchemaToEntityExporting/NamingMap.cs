using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting
{
    public class NamingMap:BOA.EntityGeneration.NamingMap
    {
        public string SlnDirectoryPath { get; set; }

        public string SchemaName => Resolve(nameof(SchemaName));
        public string RepositoryNamespace => Resolve(nameof(RepositoryNamespace));
        public string EntityProjectDirectory => Resolve(nameof(EntityProjectDirectory));
        public string RepositoryProjectDirectory => Resolve(nameof(RepositoryProjectDirectory));
        
    }
}