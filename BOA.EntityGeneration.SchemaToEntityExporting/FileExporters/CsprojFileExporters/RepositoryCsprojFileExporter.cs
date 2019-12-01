using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;

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
            foreach (var item in SchemaExporter.Config.RepositoryAssemblyReferences)
            {
                Context.RepositoryAssemblyReferences.Add(Resolve(item));

            }
            

            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = Context.RepositoryProjectSourceFileNames,
                NamespaceName    = NamingMap.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = NamingMap.RepositoryProjectDirectory,
                References       = Context.RepositoryAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);

            
        }
        #endregion
    }
}