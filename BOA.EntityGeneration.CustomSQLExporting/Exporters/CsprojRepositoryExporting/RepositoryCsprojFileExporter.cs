using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojRepositoryExporting
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
                NamespaceName    = ProfileNamingPattern.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = ProfileNamingPattern.RepositoryProjectDirectory,
                References       = RepositoryAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}