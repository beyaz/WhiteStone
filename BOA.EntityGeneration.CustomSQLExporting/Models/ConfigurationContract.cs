using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Models
{
    /// <summary>
    ///     The configuration contract
    /// </summary>
    [Serializable]
    public class ConfigurationContract
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the custom SQL get SQL item information.
        /// </summary>
        public string CustomSQL_Get_SQL_Item_Info { get; set; }

        /// <summary>
        ///     Gets or sets the custom SQL names defined to profile SQL.
        /// </summary>
        public string CustomSQLNamesDefinedToProfileSql { get; set; }

        /// <summary>
        ///     Gets or sets the custom SQL naming pattern.
        /// </summary>
        public IReadOnlyDictionary<string, string> CustomSqlNamingPattern { get; set; }

        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        /// <summary>
        ///     Gets or sets the entity contract base.
        /// </summary>
        public string EntityContractBase { get; set; }

        /// <summary>
        ///     Gets or sets the profile naming pattern.
        /// </summary>
        public IReadOnlyDictionary<string, string> ProfileNamingPattern { get; set; }

        /// <summary>
        ///     Gets or sets the SQL get profile identifier list.
        /// </summary>
        public string SQL_GetProfileIdList { get; set; }

        /// <summary>
        ///     Gets or sets the SQL sequence information of table.
        /// </summary>
        public string SqlSequenceInformationOfTable { get; set; }
        #endregion
    }
}