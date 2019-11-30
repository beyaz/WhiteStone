using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting
{
    class BoaRepositoryFileExporterConfig
    {
        #region Public Properties
        public Dictionary<string, string> DefaultValuesForInsertMethod      { get; set; }
        public Dictionary<string, string> DefaultValuesForUpdateByKeyMethod { get; set; }
        public string[] ExcludedColumnNamesWhenInsertOperation { get; set; }
        public string EmbeddedCodes { get; set; }
        public string[] ExtraAssemblyReferences { get; set; }
        #endregion


        public static BoaRepositoryFileExporterConfig CreateFromFile()
        {
            var resourceDirectoryPath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(BankingRepositoryFileExporting)}{Path.DirectorySeparatorChar}";

            return YamlHelper.DeserializeFromFile<BoaRepositoryFileExporterConfig>(resourceDirectoryPath + nameof(BoaRepositoryFileExporterConfig) + ".yaml");
        }
    }
}