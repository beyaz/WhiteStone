using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Models
{
    /// <summary>
    ///     The configuration contract
    /// </summary>
    [Serializable]
    public class ConfigContract
    {
        #region Public Properties
        

        

        /// <summary>
        ///     Gets or sets the naming pattern.
        /// </summary>
        public IReadOnlyDictionary<string, string> NamingPattern { get; set; }

       

        /// <summary>
        ///     Gets or sets the not exportable tables.
        /// </summary>
        public IReadOnlyCollection<string> NotExportableTables { get; set; }
        
        /// <summary>
        ///     Gets or sets the SQL sequence information of table.
        /// </summary>
        public string SqlSequenceInformationOfTable { get; set; }


        public bool IntegrateWithTFSAndCheckInAutomatically { get; set; }

        /// <summary>
        ///     Gets or sets the table naming pattern.
        /// </summary>
        public IReadOnlyDictionary<string, string> TableNamingPattern { get; set; }


        
        #endregion
    }
}