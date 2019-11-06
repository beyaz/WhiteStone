using System.Collections.Generic;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models
{
    public interface ICustomSqlInfo
    {
        /// <summary>
        ///     Gets the name of the business class.
        /// </summary>
        string BusinessClassName { get; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets the name of the parameter contract.
        /// </summary>
        string ParameterContractName { get; }

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        IReadOnlyList<ICustomSqlInfoParameter> Parameters { get; set; }

        string ProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the result columns.
        /// </summary>
        IReadOnlyList<CustomSqlInfoResult> ResultColumns { get; set; }

        /// <summary>
        ///     Gets the name of the result contract.
        /// </summary>
        string ResultContractName { get; }

        /// <summary>
        ///     Gets or sets the name of the schema.
        /// </summary>
        string SchemaName { get; set; }

        /// <summary>
        ///     Gets or sets the SQL.
        /// </summary>
        string Sql { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [SQL result is collection].
        /// </summary>
        bool SqlResultIsCollection { get; set; }

        int SwitchCaseIndex { get; set; }
    }
}