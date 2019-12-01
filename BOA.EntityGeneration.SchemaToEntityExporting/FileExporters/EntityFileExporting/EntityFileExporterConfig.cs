using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting
{
    class EntityFileExporterConfig
    {
        #region Public Properties
        public string[] AssemblyReferences { get; set; }
        public string   ClassName          { get; set; }
        public string   EntityContractBase { get; set; }

        public string NamespaceName { get; set; }

        public string OutputFilePath { get; set; }

        public ICollection<string> UsingLines { get; set; }
        #endregion

        #region Public Methods
        public static EntityFileExporterConfig LoadFromFile()
        {
            var filePath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(EntityFileExporting)}{Path.DirectorySeparatorChar}{nameof(EntityFileExporterConfig)}.yaml";

            return LoadFromFile(filePath);
        }

        public static EntityFileExporterConfig LoadFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<EntityFileExporterConfig>(filePath);
        }
        #endregion
    }
}