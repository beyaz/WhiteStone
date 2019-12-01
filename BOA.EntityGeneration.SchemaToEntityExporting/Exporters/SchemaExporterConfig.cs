using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Exporters
{
    /// <summary>
    ///     The schema exporter configuration
    /// </summary>
    class SchemaExporterConfig
    {
        #region Public Properties
        public bool CanExportAllSchemaInOneClassRepository { get; set; }
        public bool CanExportBoaRepository                 { get; set; }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        public string EntityProjectDirectory { get; set; }

        /// <summary>
        ///     Gets or sets the not exportable tables.
        /// </summary>
        public string[] NotExportableTables { get; set; }

        public IList<string> RepositoryAssemblyReferences { get; set; }

        public string RepositoryNamespace        { get; set; }
        public string RepositoryProjectDirectory { get; set; }
        public string SlnDirectoryPath           { get; set; }

        /// <summary>
        ///     Gets or sets the SQL sequence information of table.
        /// </summary>
        public string SqlSequenceInformationOfTable { get; set; }

        /// <summary>
        ///     Gets or sets the table catalog.
        /// </summary>
        public string TableCatalog { get; set; }

        /// <summary>
        ///     Gets or sets the table naming pattern.
        /// </summary>
        public Dictionary<string, string> TableNamingPattern { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Creates from file.
        /// </summary>
        public static SchemaExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<SchemaExporterConfig>(filePath);
        }

        public static SchemaExporterConfig CreateFromFile()
        {
            return CreateFromFile($"{nameof(Exporters)}{Path.DirectorySeparatorChar}{nameof(SchemaExporterConfig)}.yaml");
        }
        #endregion
    }
}