using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters
{
    class EntityCsprojFileExporter : ContextContainer
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
                FileNames        = Context.EntityProjectSourceFileNames,
                NamespaceName    = namingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = namingPattern.EntityProjectDirectory,
                References       = namingPattern.EntityAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}