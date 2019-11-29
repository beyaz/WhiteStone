using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.SharedFileExporting
{
    class SharedFileExporterConfig:ConfigBase
    {
        /// <summary>
        ///     Gets or sets the contract read line.
        /// </summary>
        public string ContractReadLine { get; set; }

        public static SharedFileExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<SharedFileExporterConfig>(ConfigDirectory + nameof(SharedFileExporterConfig) + ".yaml");
        }
    }
}