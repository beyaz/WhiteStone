using System.Collections.Generic;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces
{
    /// <summary>
    ///     The custom SQL information
    /// </summary>
    public interface ICustomSqlInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets the name of the business class.
        /// </summary>
        string BusinessClassName { get; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the name of the parameter contract.
        /// </summary>
        string ParameterContractName { get; }

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        IReadOnlyList<ICustomSqlInfoParameter> Parameters { get; }

        /// <summary>
        ///     Gets or sets the profile identifier.
        /// </summary>
        string ProfileId { get; }

        /// <summary>
        ///     Gets or sets the result columns.
        /// </summary>
        IReadOnlyList<ICustomSqlInfoResult> ResultColumns { get; }

        /// <summary>
        ///     Gets the name of the result contract.
        /// </summary>
        string ResultContractName { get; }

        /// <summary>
        ///     Gets or sets the name of the schema.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        ///     Gets or sets the SQL.
        /// </summary>
        string Sql { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether [SQL result is collection].
        /// </summary>
        bool SqlResultIsCollection { get; }

        /// <summary>
        ///     Gets or sets the index of the switch case.
        /// </summary>
        int SwitchCaseIndex { get; }
        #endregion
    }
}