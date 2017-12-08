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
        /// <summary>
        ///     Converts <paramref name="dataReader" /> value to DataTable instance.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
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
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IList<TContract> ToList<TContract>(this IDataReader dataReader) where TContract : class, new()
        {
            return ToDataTable(dataReader).ToList<TContract>();
        }

        internal class DataReaderAdapter : DataAdapter
        {
            public void FillFromReader(DataTable dataTable, IDataReader dataReader)
            {
                Fill(dataTable, dataReader);
            }
        }
    }
}