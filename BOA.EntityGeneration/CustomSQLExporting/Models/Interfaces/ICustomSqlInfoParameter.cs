using System.Data;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces
{
    /// <summary>
    ///     The read only custom SQL information parameter
    /// </summary>
    public interface ICustomSqlInfoParameter
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the c sharp property.
        /// </summary>
        string CSharpPropertyName { get; }

        /// <summary>
        ///     Gets or sets the name of the c sharp property type.
        /// </summary>
        string CSharpPropertyTypeName { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets or sets the name of the SQL database type.
        /// </summary>
        SqlDbType SqlDbTypeName { get; }

        /// <summary>
        ///     Gets the value access path for add in parameter.
        /// </summary>
        string ValueAccessPathForAddInParameter { get; }
        #endregion
    }
}