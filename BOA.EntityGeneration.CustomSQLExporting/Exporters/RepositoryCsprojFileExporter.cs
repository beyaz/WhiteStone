using System.Collections.Generic;
using BOA.DataFlow;
using BOA.Tasks;
using static BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters.MsBuildQueue;
using static BOA.EntityGeneration.CustomSQLExporting.Data;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public class RepositoryCsprojFileExporter
    {
        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeAssemblyReferences);
            context.AttachEvent(OnProfileInfoRemove, Export);
        }
        #endregion

        #region Methods
        static void Export(IDataContext context)
        {
            var profileNamingPattern = context.Get(ProfileNamingPattern);

            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileNames        = new List<string> {"Shared.cs", "Boa.cs"},
                NamespaceName    = profileNamingPattern.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = profileNamingPattern.RepositoryProjectDirectory,
                References       = RepositoryAssemblyReferences[context]
            };

            var csprojFilePath = csprojFileGenerator.Generate(context);

            context.Get(MsBuildQueueId).Push(new MSBuildData {ProjectFilePath = csprojFilePath});
        }

        static void InitializeAssemblyReferences(IDataContext context)
        {
            RepositoryAssemblyReferences[context] = new List<string>();
            RepositoryAssemblyReferences[context].AddRange(ProfileNamingPattern[context].RepositoryAssemblyReferences);
        }
        #endregion
    }
}