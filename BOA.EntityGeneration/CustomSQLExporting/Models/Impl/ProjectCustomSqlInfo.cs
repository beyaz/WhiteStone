using System;
using System.Collections.Generic;
using BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Impl
{

   
    [Serializable]
    public class CustomSqlProfileInfo : ICustomSqlProfileInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the business project path.
        /// </summary>
        public string BusinessProjectPath { get; set; }
        
        /// <summary>
        ///     Gets or sets the namespace name of business.
        /// </summary>
        public string NamespaceNameOfBusiness { get; set; }

        /// <summary>
        ///     Gets or sets the type of the namespace name of.
        /// </summary>
        public string NamespaceNameOfType { get; set; }

        /// <summary>
        ///     Gets or sets the profile identifier.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the types project path.
        /// </summary>
        public string TypesProjectPath { get; set; }
        #endregion

        #region Explicit Interface Properties
        public IReadOnlyList<string> ObjectIdList { get; set; }
        #endregion
    }


   
}