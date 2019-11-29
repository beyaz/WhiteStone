using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting
{
    class EntityFileExporterConfig:ConfigBase
    {
        /// <summary>
        ///     Gets or sets the entity contract base.
        /// </summary>
        public string EntityContractBase { get; set; }


        public static EntityFileExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<EntityFileExporterConfig>(ConfigDirectory + nameof(EntityFileExporterConfig) + ".yaml");
        }
    }
}