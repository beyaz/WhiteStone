using System.Collections.Generic;
using BOA.Tasks;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityCsprojFileExporter : ContextContainer
    {
        #region Public Methods
        public void AttachEvents()
        {
            AttachEvent(ProfileInfoRemove, Export);
        }
        #endregion

        #region Methods
        void Export()
        {
            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem = FileSystem,
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = profileNamingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = profileNamingPattern.EntityProjectDirectory,
                References       = entityAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }

        
        #endregion
    }
}