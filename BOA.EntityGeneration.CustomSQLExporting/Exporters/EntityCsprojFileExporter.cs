using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.Tasks;
using static BOA.EntityGeneration.CustomSQLExporting.Data;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityCsprojFileExporter
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
            var profileNamingPattern = ProfileNamingPattern[context];

            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = profileNamingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = profileNamingPattern.EntityProjectDirectory,
                References       = EntityAssemblyReferences[context]
            };

            var csprojFilePath = csprojFileGenerator.Generate(context);

            context.Get(MsBuildQueue.MsBuildQueueId).Push(new MSBuildData {ProjectFilePath = csprojFilePath});
        }

        static void InitializeAssemblyReferences(IDataContext context)
        {
            EntityAssemblyReferences[context] = new List<string>();
            EntityAssemblyReferences[context].AddRange(ProfileNamingPattern[context].EntityAssemblyReferences);
        }
        #endregion
    }
}