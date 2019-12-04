﻿using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojRepositoryExporting
{
    class RepositoryCsprojFileExporterConfig
    {
        #region Public Properties
        public IList<string> DefaultAssemblyReferences { get; set; }
        #endregion

        #region Public Methods
        public static RepositoryCsprojFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<RepositoryCsprojFileExporterConfig>(filePath);
        }

        public static RepositoryCsprojFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(), nameof(Exporters), nameof(CsprojRepositoryExporting), nameof(RepositoryCsprojFileExporterConfig) + ".yaml"));
        }
        #endregion
    }
}