namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters
{
    class RepositoryCsprojFileExporter : ContextContainer
    {
        static readonly RepositoryCsprojFileExporterConfig Config = RepositoryCsprojFileExporterConfig.CreateFromFile();

        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportFinished += Export;
        }
        #endregion


        #region Methods
        void Export()
        {
            foreach (var item in Config.DefaultAssemblyReferences)
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