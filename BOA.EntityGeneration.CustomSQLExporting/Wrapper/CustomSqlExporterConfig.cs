using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    class CustomSqlExporterConfig
    {
        public string SlnDirectoryPath { get; set; }
        public string EntityNamespace { get; set; }
        public string RepositoryNamespace { get; set; }


        public static CustomSqlExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<CustomSqlExporterConfig>(filePath);
        }

        public static CustomSqlExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(),nameof(Wrapper),nameof(CustomSqlExporterConfig)+".yaml"));
        }
    }
}