using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityCsprojFileExporter : ContextContainer
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
                FileNames        = new List<string> {"All.cs"},
                NamespaceName    = ProfileNamingPattern.EntityNamespace,
                IsClientDll      = true,
                ProjectDirectory = ProfileNamingPattern.EntityProjectDirectory,
                References       = EntityAssemblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}