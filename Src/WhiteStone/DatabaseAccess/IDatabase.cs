using System;
using System.Data;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     Manager interface of sql operations
    /// </summary>
    public interface IDatabase : IDisposable
    {
        #region Public Properties
        /// <summary>
        ///     Indicates CommandType is StoredProcedure or Text
        /// </summary>
        /// <returns></returns>
        bool CommandIsStoredProcedure { set; get; }

        /// <summary>
        ///     Command that will be execute
        /// </summary>
        string CommandText { set; get; }

        /// <summary>
        ///     Command timeout value
        /// </summary>
        int? CommandTimeout { set; get; }

        /// <summary>
        ///     Parameter prefix for sql sentences.
        /// </summary>
        string ParameterPrefix { get; }
        #endregion

        #region Public Indexers
        /// <summary>
        ///     Adds a parameter to sql command.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        object this[string parameterName] { set; get; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Begins database transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        ///     Commits transaction
        /// </summary>
        void Commit();

        /// <summary>
        ///     Executes command and returns effected row counts
        /// </summary>
        /// <returns></returns>
        int ExecuteNonQuery();

        /// <summary>
        ///     Executes command and returns reader
        /// </summary>
        /// <returns></returns>
        IDataReader ExecuteReader();

        /// <summary>
        ///     Executes commands ands returns command value.
        /// </summary>
        /// <returns></returns>
        object ExecuteScalar();

        /// <summary>
        ///     Rollback transaction
        /// </summary>
        void Rollback();
        #endregion
    }
}