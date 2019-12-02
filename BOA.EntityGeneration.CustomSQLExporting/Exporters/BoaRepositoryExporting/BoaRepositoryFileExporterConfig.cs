using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.BoaRepositoryExporting
{
    class BoaRepositoryFileExporterConfig
    {
        #region Public Properties
        public string EmbeddedCodes { get; set; }

        public string[] UsingLines { get; set; }
        public string OutputFilePath { get; set; }
        #endregion

        #region Public Methods
        public static BoaRepositoryFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<BoaRepositoryFileExporterConfig>(filePath);
        }

        public static BoaRepositoryFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(),nameof(Exporters),nameof(BoaRepositoryExporting),nameof(BoaRepositoryFileExporterConfig)+".yaml"));
        }
        #endregion
    }
}