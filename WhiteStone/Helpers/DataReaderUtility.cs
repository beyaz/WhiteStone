using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using BOA.Common.Helpers;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Utility methods for <see cref="IDataReader" />
    /// </summary>
    public static class DataReaderUtility
    {
        #region Public Methods
        /// <summary>
        ///     Fors the each row.
        /// </summary>
        public static void ForEachRow(this IDataReader dataReader, Action<IDataReader> action)
        {
            while (dataReader.Read())
            {
                action(dataReader);
            }
        }

        /// <summary>
        ///     Converts <paramref name="dataReader" /> value to DataTable instance.
        /// </summary>
        public static DataTable ToDataTable(this IDataReader dataReader)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }

            var outputTable = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            var adapter = new DataReaderAdapter();
            adapter.FillFromReader(outputTable, dataReader);

            dataReader.Close();

            return outputTable;
        }

        /// <summary>
        ///     Converts <paramref name="dataReader" /> parameter to list of <code>TContract</code>
        /// </summary>
        public static List<TContract> ToList<TContract>(this IDataReader dataReader) where TContract : class, new()
        {
            return ToDataTable(dataReader).ToList<TContract>();
        }
        #endregion

        /// <summary>
        ///     The data reader adapter
        /// </summary>
        internal class DataReaderAdapter : DataAdapter
        {
            #region Public Methods
            /// <summary>
            ///     Fills from reader.
            /// </summary>
            public void FillFromReader(DataTable dataTable, IDataReader dataReader)
            {
                Fill(dataTable, dataReader);
            }
            #endregion
        }
    }
}