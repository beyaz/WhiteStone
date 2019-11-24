using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.Tasks;

namespace BOA.EntityGeneration.CsprojFileExporters
{
    class EntityCsprojFileExporter : ContextContainer
    {
        #region Public Methods
        public void AttachEvents()
        {
            OnSchemaExportFinished+= Export;
        }
        #endregion

        #region Methods
        void Export()
        {
            var csprojFileGenerator = new CsprojFileGenerator
            {
                Context          = Context,
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = namingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = namingPattern.EntityProjectDirectory,
                References       = namingPattern.EntityAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(new MSBuildData {ProjectFilePath = csprojFilePath});
        }
        #endregion
    }
}