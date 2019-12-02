using System.IO;
using BOA.Common.Helpers;
using Config = BOA.EntityGeneration.CustomSQLExporting.Exporters.SharedFileExporting.SharedFileExporterConfig;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.SharedFileExporting
{
    class SharedFileExporterConfig
    {
        #region Public Properties
        public string EmbeddedCodes { get; set; }
        #endregion

        #region Public Methods
        public static Config CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<Config>(filePath);
        }

        public static Config CreateFromFile()
        {
            return CreateFromFile($"{nameof(Exporters)}{Path.DirectorySeparatorChar}{nameof(SharedFileExporting)}{Path.DirectorySeparatorChar}{nameof(SharedFileExporterConfig)}.yaml");
        }
        #endregion
    }
}