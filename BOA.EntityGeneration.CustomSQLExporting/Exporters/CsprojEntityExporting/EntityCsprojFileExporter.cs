using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.ContextManagement;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojEntityExporting
{
    class EntityCsprojFileExporter : ContextContainer
    {
        #region Static Fields
        static readonly EntityCsprojFileExporterConfig _config = EntityCsprojFileExporterConfig.CreateFromFile();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoRemove += Export;
        }
        #endregion

        #region Methods
        void Export()
        {
            var references = new List<string>();

            foreach (var reference in _config.DefaultAssemblyReferences)
            {
                references.Add(Resolve(reference));
            }

            references.AddRange(ExtraAssemblyReferencesForEntityProject);

            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = NamingMap.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = Context.NamingMap.EntityProjectDirectory,
                References       = references
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}