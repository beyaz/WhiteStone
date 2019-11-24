using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class RepositoryCsprojFileExporter : ContextContainer
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
            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = new List<string> {"Shared.cs", "Boa.cs"},
                NamespaceName    = profileNamingPattern.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = profileNamingPattern.RepositoryProjectDirectory,
                References       = repositoryAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}