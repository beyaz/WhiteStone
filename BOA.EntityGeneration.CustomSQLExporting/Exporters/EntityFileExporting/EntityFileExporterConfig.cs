using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.EntityFileExporting
{
    class EntityFileExporterConfig
    {
        #region Public Properties
        public string        OutputFilePath { get; set; }
        public IList<string> UsingLines     { get; set; }
        #endregion

        #region Public Methods
        public static EntityFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<EntityFileExporterConfig>(filePath);
        }

        public static EntityFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(), nameof(Exporters), nameof(EntityFileExporting), nameof(EntityFileExporterConfig) + ".yaml"));
        }
        #endregion
    }
}