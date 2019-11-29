using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting
{
    class BoaRepositoryFileExporterConfig:ConfigBase
    {
       public Dictionary<string, string> DefaultValuesForInsertMethod { get; set; }
       public Dictionary<string, string> DefaultValuesForUpdateByKeyMethod { get; set; }

        public static BoaRepositoryFileExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<BoaRepositoryFileExporterConfig>(ConfigDirectory + nameof(BoaRepositoryFileExporterConfig) + ".yaml");
        }
    }
}