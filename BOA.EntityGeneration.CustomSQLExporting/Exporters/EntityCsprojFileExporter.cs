using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.Tasks;
using static BOA.EntityGeneration.CustomSQLExporting.Data;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityCsprojFileExporter:ContextContainer
    {
        #region Public Methods
        public  void AttachEvents()
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
                Context=Context,
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = profileNamingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = profileNamingPattern.EntityProjectDirectory,
                References       = entityAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(new MSBuildData {ProjectFilePath = csprojFilePath});
        }

         void InitializeAssemblyReferences()
        {
            EntityAssemblyReferences[Context] = new List<string>();
            EntityAssemblyReferences[Context].AddRange(profileNamingPattern.EntityAssemblyReferences);
        }
        #endregion
    }
}