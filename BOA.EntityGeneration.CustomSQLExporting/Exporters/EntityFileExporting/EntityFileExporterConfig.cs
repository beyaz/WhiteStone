using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.EntityFileExporting
{
    class EntityFileExporterConfig
    {
        public IList<string> UsingLines { get; set; }
        public IList<string> DefaultAssemblyReferences { get; set; }

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