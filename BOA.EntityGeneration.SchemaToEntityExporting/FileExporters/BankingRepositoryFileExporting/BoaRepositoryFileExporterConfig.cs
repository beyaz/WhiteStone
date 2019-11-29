using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class BoaRepositoryFileExporterConfig:ConfigBase
    {
       public IReadOnlyDictionary<string, string> DefaultValuesForInsertMethod { get; set; }
       public IReadOnlyDictionary<string, string> DefaultValuesForUpdateByKeyMethod { get; set; }

        public static BoaRepositoryFileExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<BoaRepositoryFileExporterConfig>(ConfigDirectory + nameof(BoaRepositoryFileExporterConfig) + ".yaml");
        }
    }
}