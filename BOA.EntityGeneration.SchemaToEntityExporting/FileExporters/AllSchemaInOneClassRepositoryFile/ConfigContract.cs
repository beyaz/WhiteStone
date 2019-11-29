using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{

    class AllSchemaInOneClassRepositoryFileExporterConfig:ConfigBase
    {
        public Dictionary<string,string> NamingPattern { get; set; }


        public static AllSchemaInOneClassRepositoryFileExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<AllSchemaInOneClassRepositoryFileExporterConfig>(ConfigDirectory + nameof(AllSchemaInOneClassRepositoryFileExporterConfig) + ".yaml");
        }
    }
}