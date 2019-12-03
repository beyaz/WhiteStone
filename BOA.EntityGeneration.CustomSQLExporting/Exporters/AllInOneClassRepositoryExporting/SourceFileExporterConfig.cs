using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.AllInOneClassRepositoryExporting
{
    class SourceFileExporterConfig
    {
        public string MethodName { get; set; }
        #region Public Properties
        public string   EmbeddedCodes                   { get; set; }
        public string   OutputFilePath                  { get; set; }
        public string   SharedRepositoryClassAccessPath { get; set; }
        public string[] UsingLines                      { get; set; }
        #endregion

        public string[] ExtraAssemblyReferences { get; set; }
        public string ClassDefinitionBegin { get; set; }
        public string ClassName { get; set; }
        public string NamespaceName { get; set; }
        #region Public Methods
        public static SourceFileExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<SourceFileExporterConfig>(filePath);
        }

        public static SourceFileExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(), nameof(Exporters), nameof(AllInOneClassRepositoryExporting), nameof(SourceFileExporterConfig) + ".yaml"));
        }
        #endregion
    }
}