using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting
{
    class EntityFileExporterConfig
    {

        public string OutputFilePath { get; set; }
        public string EntityContractBase { get; set; }

        public string NamespaceName { get; set; }
        public string ClassName { get; set; }

        public ICollection<string> UsingLines { get; set; }

        public string[] AssemblyReferences { get; set; }

        public static EntityFileExporterConfig LoadFromFile()
        {
            var resourceDirectoryPath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(EntityFileExporting)}{Path.DirectorySeparatorChar}";

            return YamlHelper.DeserializeFromFile<EntityFileExporterConfig>(resourceDirectoryPath + nameof(EntityFileExporterConfig) + ".yaml");
        }
        
    }
}