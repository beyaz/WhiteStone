using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters
{
    class RepositoryCsprojFileExporterConfig
    {
        public IList<string> DefaultAssemblyReferences { get; set; }

        public static RepositoryCsprojFileExporterConfig CreateFromFile()
        {
            var separatorChar = Path.DirectorySeparatorChar;

            return YamlHelper.DeserializeFromFile<RepositoryCsprojFileExporterConfig>($"{nameof(FileExporters)}{separatorChar}{nameof(CsprojFileExporters)}{separatorChar}{nameof(RepositoryCsprojFileExporterConfig)}.yaml");
        }
    }
}