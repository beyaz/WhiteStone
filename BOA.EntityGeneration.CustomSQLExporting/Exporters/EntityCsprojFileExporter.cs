using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Exporters.EntityFileExporting;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityCsprojFileExporter : ContextContainer
    {
        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoRemove += Export;
        }
        #endregion

        #region Methods
        void Export()
        {
            var references  = new List<string>();

            foreach (var reference in EntityFileExporter._config.DefaultAssemblyReferences)
            {
                references.Add(Resolve(reference));
            }

            references.AddRange(ExtraAssemblyReferencesForEntityProject);

            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = ProfileNamingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = ProfileNamingPattern.EntityProjectDirectory,
                References       = references
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}