using System;
using System.Collections.Generic;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Impl
{
    /// <summary>
    ///     The project custom SQL information
    /// </summary>
    [Serializable]
    public class ProjectCustomSqlInfo : IProjectCustomSqlInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the business project path.
        /// </summary>
        public string BusinessProjectPath { get; set; }

        /// <summary>
        ///     Gets or sets the custom SQL information list.
        /// </summary>
        public List<CustomSqlInfo> CustomSqlInfoList { get; set; }

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
        /// <summary>
        ///     Gets the custom SQL information list.
        /// </summary>
        IReadOnlyList<ICustomSqlInfo> IProjectCustomSqlInfo.CustomSqlInfoList => CustomSqlInfoList;

        public List<string> ObjectIdList { get; set; }
        #endregion
    }
}