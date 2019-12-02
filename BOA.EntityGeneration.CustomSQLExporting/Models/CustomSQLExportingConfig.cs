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
        ///     Gets or sets the SQL get profile identifier list.
        /// </summary>
        public string SQL_GetProfileIdList { get; set; }

       
        #endregion
    }
}