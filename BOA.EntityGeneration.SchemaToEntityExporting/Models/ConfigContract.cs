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
        ///     Gets or sets the table naming pattern.
        /// </summary>
        public IReadOnlyDictionary<string, string> TableNamingPattern { get; set; }


        
        #endregion
    }
}