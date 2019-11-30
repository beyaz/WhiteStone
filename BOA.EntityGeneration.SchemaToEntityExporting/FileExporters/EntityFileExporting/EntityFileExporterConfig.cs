using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting
{
    class EntityFileExporterConfig
    {
        /// <summary>
        ///     Gets or sets the entity contract base.
        /// </summary>
        public string EntityContractBase { get; set; }

        public string[] UsingLines { get; set; }

        public static EntityFileExporterConfig LoadFromFile()
        {
            var resourceDirectoryPath = $"{nameof(FileExporters)}{Path.DirectorySeparatorChar}{nameof(EntityFileExporting)}{Path.DirectorySeparatorChar}";

            return YamlHelper.DeserializeFromFile<EntityFileExporterConfig>(resourceDirectoryPath + nameof(EntityFileExporterConfig) + ".yaml");
        }
        
    }
}