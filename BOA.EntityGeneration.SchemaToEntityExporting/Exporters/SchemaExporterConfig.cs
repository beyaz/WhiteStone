using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Exporters
{
    /// <summary>
    ///     The schema exporter configuration
    /// </summary>
    class SchemaExporterConfig : ConfigBase
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        /// <summary>
        ///     Gets or sets the naming pattern.
        /// </summary>
        public Dictionary<string, string> NamingPattern { get; set; }

        /// <summary>
        ///     Gets or sets the not exportable tables.
        /// </summary>
        public string[] NotExportableTables { get; set; }

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
        public static SchemaExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<SchemaExporterConfig>(ConfigDirectory + nameof(SchemaExporterConfig) + ".yaml");
        }
        #endregion
    }
}