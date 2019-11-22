using System.Collections.Generic;
using BOA.Tasks;
using static BOA.EntityGeneration.CustomSQLExporting.Data;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class RepositoryCsprojFileExporter : ContextContainer
    {
        #region Public Methods
        public void AttachEvents()
        {
            AttachEvent(OnProfileInfoInitialized, InitializeAssemblyReferences);
            AttachEvent(OnProfileInfoRemove, Export);
        }
        #endregion

        #region Methods
        void Export()
        {
            var csprojFileGenerator = new CsprojFileGenerator
            {
                Context          = Context,
                FileNames        = new List<string> {"Shared.cs", "Boa.cs"},
                NamespaceName    = profileNamingPattern.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = profileNamingPattern.RepositoryProjectDirectory,
                References       = repositoryAssemblyReference
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(new MSBuildData {ProjectFilePath = csprojFilePath});
        }

        void InitializeAssemblyReferences()
        {
            RepositoryAssemblyReferences[Context] = new List<string>();
            repositoryAssemblyReferences.AddRange(profileNamingPattern.RepositoryAssemblyReferences);
        }
        #endregion
    }
}