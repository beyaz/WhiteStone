using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.CustomSQLExporting.Exporters.BoaRepositoryExporting;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.EntityFileExporting
{
    class EntityFileExporterConfig
    {
        public IList<string> UsingLines { get; set; }

        public static EntityFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<EntityFileExporterConfig>(filePath);
        }

        public static EntityFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(),nameof(Exporters),nameof(EntityFileExporting),nameof(EntityFileExporterConfig)+".yaml"));
        }
    }
}