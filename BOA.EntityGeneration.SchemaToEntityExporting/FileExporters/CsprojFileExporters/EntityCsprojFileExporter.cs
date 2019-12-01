using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;

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
                NamespaceName    =  NamingMap.Resolve(NamingMapKey.EntityNamespaceName),
                IsClientDll      = true,
                ProjectDirectory = NamingPattern.EntityProjectDirectory,
                References       = EntityFileExporter.Config.AssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}