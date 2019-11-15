using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces
{
    /// <summary>
    ///     The custom SQL information result
    /// </summary>
    public interface ICustomSqlInfoResult
    {
        #region Public Properties
        /// <summary>
        ///     Gets the type of the data.
        /// </summary>
        string DataType { get; }

        /// <summary>
        ///     Gets the data type in dotnet.
        /// </summary>
        string DataTypeInDotnet { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is nullable.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the name in dotnet.
        /// </summary>
        string NameInDotnet { get; }

        /// <summary>
        ///     Gets the SQL reader method.
        /// </summary>
        SqlReaderMethods SqlReaderMethod { get; }
        #endregion
    }
}