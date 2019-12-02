using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojEntityExporting
{
    class EntityCsprojFileExporterConfig
    {
        #region Public Properties
        public IList<string> DefaultAssemblyReferences { get; set; }
        #endregion

        #region Public Methods
        public static EntityCsprojFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<EntityCsprojFileExporterConfig>(filePath);
        }

        public static EntityCsprojFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(), nameof(Exporters), nameof(CsprojEntityExporting), nameof(EntityCsprojFileExporterConfig) + ".yaml"));
        }
        #endregion
    }
}