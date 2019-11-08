using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration
{
    /// <summary>
    ///     The configuration
    /// </summary>
    [Serializable]
    public class Config
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets a value indicating whether [build after code generation is completed].
        /// </summary>
        public bool BuildAfterCodeGenerationIsCompleted { get; set; }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the DAO assembly namespace format.
        /// </summary>
        public string DaoAssemblyNamespaceFormat { get; set; }

        /// <summary>
        ///     Gets or sets the DAO using lines.
        /// </summary>
        public IReadOnlyCollection<string> DaoUsingLines { get; set; }

        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [enable full project export].
        /// </summary>
        public bool EnableFullProjectExport { get; set; }

        /// <summary>
        ///     Gets or sets the entity assembly namespace format.
        /// </summary>
        public string EntityAssemblyNamespaceFormat { get; set; }

        /// <summary>
        ///     Gets or sets the file path for all DAO in one file.
        /// </summary>
        public string FilePathForAllDaoInOneFile { get; set; }

        /// <summary>
        ///     Gets or sets the file path for all entities in one file.
        /// </summary>
        public string FilePathForAllEntitiesInOneFile { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [integrate with boa TFS].
        /// </summary>
        public bool IntegrateWithBOATfs { get; set; }

        /// <summary>
        ///     Gets or sets the not exportable tables.
        /// </summary>
        public IReadOnlyCollection<string> NotExportableTables { get; set; }

        /// <summary>
        ///     Gets or sets the read line character to bool.
        /// </summary>
        public string ReadLineCharToBool { get; set; }

        /// <summary>
        ///     Gets or sets the read line character to bool nullable.
        /// </summary>
        public string ReadLineCharToBoolNullable { get; set; }

        /// <summary>
        ///     Gets or sets the read line default.
        /// </summary>
        public string ReadLineDefault { get; set; }

        /// <summary>
        ///     Gets or sets the schema names to be export.
        /// </summary>
        public IReadOnlyCollection<string> SchemaNamesToBeExport { get; set; }

        /// <summary>
        ///     Gets or sets the SLN directory path.
        /// </summary>
        public string SlnDirectoryPath { get; set; }

        /// <summary>
        ///     Gets or sets the SQL sequence information of table.
        /// </summary>
        public string SqlSequenceInformationOfTable { get; set; }

        /// <summary>
        ///     Gets or sets the table catalog.
        /// </summary>
        public string TableCatalog { get; set; }

        /// <summary>
        ///     Gets or sets the type contract base.
        /// </summary>
        public string TypeContractBase { get; set; }

        /// <summary>
        ///     Gets or sets the type using lines.
        /// </summary>
        public IReadOnlyCollection<string> TypeUsingLines { get; set; }
        #endregion
    }
}