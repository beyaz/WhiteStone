using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces
{

    
    
    public interface ICustomSqlProfileInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets the business project path.
        /// </summary>
        string BusinessProjectPath { get; }

        /// <summary>
        ///     Gets the namespace name of business.
        /// </summary>
        string NamespaceNameOfBusiness { get; }

        /// <summary>
        ///     Gets the type of the namespace name of.
        /// </summary>
        string NamespaceNameOfType { get; }

        /// <summary>
        ///     Gets the profile identifier.
        /// </summary>
        string ProfileId { get; }

        /// <summary>
        ///     Gets the types project path.
        /// </summary>
        string TypesProjectPath { get; }

        IReadOnlyList<string> ObjectIdList { get; set; }
        #endregion
    }

   
}