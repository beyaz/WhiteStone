using System;
using System.Data;

namespace WhiteStone.Data
{
    /// <summary>
    ///     Manager interface of sql operations
    /// </summary>
    public interface IDatabase : IDisposable
    {
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

        /// <summary>
        ///     Adds a parameter to sql command.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        object this[string parameterName] { set; get; }

        /// <summary>
        ///     Indicates CommandType is StoredProcedure or Text
        /// </summary>
        /// <returns></returns>
        bool CommandIsStoredProcedure { set; get; }

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
        ///     Executes command and returns effected row counts
        /// </summary>
        /// <returns></returns>
        int ExecuteNonQuery();

       

        /// <summary>
        ///     Begins database transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        ///     Commits transaction
        /// </summary>
        void Commit();

        /// <summary>
        ///     Rollback transaction
        /// </summary>
        void Rollback();

        #region DML
        /// <summary>
        ///     Inserts the specified entity contract.
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        void Insert(object entityContract);

        /// <summary>
        ///     Deletes the specified entity contract.
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        void Delete(object entityContract);

        /// <summary>
        ///     Updates the specified entity contract.
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        void Update(object entityContract);

        /// <summary>
        ///     Selects the specified entity contract with given entity contract primary columns
        /// </summary>
        /// <param name="entityContract">The entity contract.</param>
        /// <returns></returns>
        void SelectEntity(object entityContract);
        #endregion
    }
}