using System.Collections.Generic;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Impl;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces
{
    /// <summary>
    ///     The project custom SQL information{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public interface IProjectCustomSqlInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets the business project path.
        /// </summary>
        string BusinessProjectPath { get; }

        /// <summary>
        ///     Gets the custom SQL information list.
        /// </summary>
        IReadOnlyList<CustomSqlInfo> CustomSqlInfoList { get; }

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
        #endregion
    }
}