using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.CsprojEntityExporting
{
    class EntityCsprojFileExporterConfig
    {
        public IList<string> DefaultAssemblyReferences { get; set; }

        public static EntityCsprojFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<EntityCsprojFileExporterConfig>(filePath);
        }

        public static EntityCsprojFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(),nameof(Exporters),nameof(CsprojEntityExporting),nameof(EntityCsprojFileExporterConfig)+".yaml"));
        }
    }
}