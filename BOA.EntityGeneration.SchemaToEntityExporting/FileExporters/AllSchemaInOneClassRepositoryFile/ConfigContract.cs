using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{
    class AllSchemaInOneClassRepositoryFileExporterConfig
    {

        public string OutputFilePath { get; set; }
        public IList<string> UsingLines              { get; set; }
        public IList<string> ExtraAssemblyReferences { get; set; }
        
        public string NamespaceName { get; set; }
        public string ClassName     { get; set; }



        public string SharedRepositoryClassAccessPath { get; set; }
        #region Public Properties
        public Dictionary<string, string> DefaultValuesForInsertMethod      { get; set; }
        public Dictionary<string, string> DefaultValuesForUpdateByKeyMethod { get; set; }
        public Dictionary<string, string> NamingPattern                     { get; set; }
        public string[] ExcludedColumnNamesWhenInsertOperation { get; set; }
        public string EmbeddedCodes { get; set; }
        #endregion

        public static AllSchemaInOneClassRepositoryFileExporterConfig CreateFromFile()
        {
            var resourceDirectoryPath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(AllSchemaInOneClassRepositoryFile)}{Path.DirectorySeparatorChar}";

            return CreateFromFile(resourceDirectoryPath + nameof(AllSchemaInOneClassRepositoryFileExporterConfig) + ".yaml");
        }

        public static AllSchemaInOneClassRepositoryFileExporterConfig CreateFromFile(string yamlFilePath)
        {

            return YamlHelper.DeserializeFromFile<AllSchemaInOneClassRepositoryFileExporterConfig>(yamlFilePath);
        }
    }
}