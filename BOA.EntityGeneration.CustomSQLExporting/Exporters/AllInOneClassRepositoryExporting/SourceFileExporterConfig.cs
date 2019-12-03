using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.AllInOneClassRepositoryExporting
{
    class SourceFileExporterConfig
    {
        #region Public Properties
        public string   EmbeddedCodes                   { get; set; }
        public string   OutputFilePath                  { get; set; }
        public string   SharedRepositoryClassAccessPath { get; set; }
        public string[] UsingLines                      { get; set; }
        #endregion

        #region Public Methods
        public static SourceFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<SourceFileExporterConfig>(filePath);
        }

        public static SourceFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(), nameof(Exporters), nameof(BoaRepositoryExporting), nameof(SourceFileExporterConfig) + ".yaml"));
        }
        #endregion
    }
}