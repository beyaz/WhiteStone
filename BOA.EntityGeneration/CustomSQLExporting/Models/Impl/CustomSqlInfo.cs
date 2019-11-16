using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Impl
{
    /// <summary>
    ///     The custom SQL information
    /// </summary>
    [Serializable]
    public class CustomSqlInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets the name of the business class.
        /// </summary>
        public string BusinessClassName => Name.ToContractName();

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the name of the parameter contract.
        /// </summary>
        public string ParameterContractName => Name.ToContractName() + "Request";

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        public IReadOnlyList<CustomSqlInfoParameter> Parameters { get; set; }

        /// <summary>
        ///     Gets or sets the profile identifier.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the result columns.
        /// </summary>
        public IReadOnlyList<CustomSqlInfoResult> ResultColumns { get; set; }

        /// <summary>
        ///     Gets the name of the result contract.
        /// </summary>
        public string ResultContractName => Name.ToContractName() + "Contract";

        /// <summary>
        ///     Gets or sets the name of the schema.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        ///     Gets or sets the SQL.
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [SQL result is collection].
        /// </summary>
        public bool SqlResultIsCollection { get; set; }

        /// <summary>
        ///     Gets or sets the index of the switch case.
        /// </summary>
        public int SwitchCaseIndex { get; set; }
        #endregion

        
    }
}