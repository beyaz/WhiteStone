using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojEntityExporting
{
    class EntityCsprojFileExporter : ContextContainer
    {
        static readonly EntityCsprojFileExporterConfig _config = EntityCsprojFileExporterConfig.CreateFromFile();

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

            foreach (var reference in _config.DefaultAssemblyReferences)
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
                ProjectDirectory = Context.NamingMap.EntityProjectDirectory,
                References       = references
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}