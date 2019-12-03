﻿using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.ContextManagement;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojRepositoryExporting
{
    class RepositoryCsprojFileExporter : ContextContainer
    {
        #region Static Fields
        internal static readonly RepositoryCsprojFileExporterConfig Config = RepositoryCsprojFileExporterConfig.CreateFromFile();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoRemove += Export;
        }
        #endregion

        #region Methods
        void Export()
        {
            var references = new List<string>();

            foreach (var reference in Config.DefaultAssemblyReferences)
            {
                references.Add(Resolve(reference));
            }

            references.AddRange(Context.ExtraAssemblyReferencesForRepositoryProject);

            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = new List<string> {"Shared.cs", "Boa.cs"},
                NamespaceName    = NamingMap.RepositoryNamespace,
                IsClientDll      = false,
                ProjectDirectory = Context.NamingMap.RepositoryProjectDirectory,
                References       = references
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            MsBuildQueue.Push(csprojFilePath);
        }
        #endregion
    }
}