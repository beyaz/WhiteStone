using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.SharedFileExporting
{
    class SharedFileExporterConfig
    {
        /// <summary>
        ///     Gets or sets the contract read line.
        /// </summary>
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