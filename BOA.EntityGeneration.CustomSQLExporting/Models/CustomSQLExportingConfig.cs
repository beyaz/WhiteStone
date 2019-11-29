using System;
using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.CustomSQLExporting.Models
{
    /// <summary>
    ///     The configuration contract
    /// </summary>
    [Serializable]
    public class CustomSQLExportingConfig
    {

        public static CustomSQLExportingConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<CustomSQLExportingConfig>( nameof(CustomSQLExportingConfig) + ".yaml");
        }

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
        public Dictionary<string, string> CustomSqlNamingPattern { get; set; }

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
        public Dictionary<string, string> ProfileNamingPattern { get; set; }

        /// <summary>
        ///     Gets or sets the SQL get profile identifier list.
        /// </summary>
        public string SQL_GetProfileIdList { get; set; }

       

        public bool IntegrateWithTFSAndCheckInAutomatically { get; set; }
        #endregion
    }
}