using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.SharedFileExporting
{
    class SharedFileExporterConfig
    {
        public string OutputFilePath { get; set; }
        public   string ClassNamePattern { get; set; }
        public string ContractReadLine { get; set; }
        public string EmbeddedCodes { get; set; }
        public ICollection<string> UsingLines { get; set; }

        public static SharedFileExporterConfig CreateFromFile()
        {
            var resourceDirectoryPath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(SharedFileExporting)}{Path.DirectorySeparatorChar}";

            return YamlHelper.DeserializeFromFile<SharedFileExporterConfig>(resourceDirectoryPath + nameof(SharedFileExporterConfig) + ".yaml");
        }
    }
}