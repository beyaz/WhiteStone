using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters
{
    class RepositoryCsprojFileExporter : ContextContainer
    {
        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportFinished += Export;
        }
        #endregion

        #region Methods
        void Export()
        {
            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = new List<string> {"Shared.cs", "Boa.cs"},
                NamespaceName    = namingPattern.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = namingPattern.RepositoryProjectDirectory,
                References       = namingPattern.RepositoryAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}