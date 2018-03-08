using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.Office.Excel
{
    /// <summary>
    ///     The copy paste string helper
    /// </summary>
    public class CopyPasteStringHelper
    {
        #region Constants
        /// <summary>
        ///     The tab charachter
        /// </summary>
        const char TabCharachter = '\t';
        #endregion

        #region Public Methods
        /// <summary>
        ///     Parses from string.
        /// </summary>
        public static IReadOnlyList<TContract> ParseFromString<TContract>(string copiedExcelData, IReadOnlyList<string> propertyNames) where TContract : new()
        {
            var table = ParseFromString(copiedExcelData);

            return table.Select(r => LineToContract<TContract>(r, propertyNames)).ToList();
        }

        /// <summary>
        ///     Parses from string.
        /// </summary>
        public static IReadOnlyList<IReadOnlyList<string>> ParseFromString(string copiedExcelData)
        {
            if (copiedExcelData == null)
            {
                throw new ArgumentNullException(nameof(copiedExcelData));
            }

            copiedExcelData = copiedExcelData.Trim();

            var rows = copiedExcelData.Split(Environment.NewLine.ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x));

            var table = new List<IReadOnlyList<string>>();

            foreach (var row in rows)
            {
                var cells = row.Split(TabCharachter);
                table.Add(cells);
            }

            return table;
        }

        /// <summary>
        ///     Parses from string.
        /// </summary>
        public static IReadOnlyList<IDictionary<string, string>> ParseFromStringAsDictionary(string copiedExcelData)
        {
            var returnList = new List<IDictionary<string, string>>();

            var table = ParseFromString(copiedExcelData);
            for (var rowIndex = 1; rowIndex < table.Count; rowIndex++)
            {
                var dictionary = new Dictionary<string, string>();

                var columnNames = table[0];

                for (var columnIndex = 0; columnIndex < columnNames.Count; columnIndex++)
                {
                    dictionary[columnNames[columnIndex].Trim()] = table[rowIndex][columnIndex];
                }

                returnList.Add(dictionary);
            }

            return returnList;
        }

        /// <summary>
        ///     Prepares for paste to excel.
        /// </summary>
        public static string PrepareForPasteToExcel<TContract>(IReadOnlyList<TContract> contracts, IReadOnlyList<string> propertyNames)
        {
            var rows = new List<List<string>>();

            foreach (var contract in contracts)
            {
                var row = new List<string>();

                foreach (var propertyName in propertyNames)
                {
                    var valueAsString = GetPropertyValueForExcelCell(contract, propertyName);

                    row.Add(valueAsString);
                }

                rows.Add(row);
            }

            return string.Join(Environment.NewLine, rows.ConvertAll(r => string.Join(TabCharachter.ToString(), r)));
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Gets the property value for excel cell.
        /// </summary>
        static string GetPropertyValueForExcelCell<TContract>(TContract contract, string propertyName)
        {
            var value = Reflection.GetPropertyValue(contract, propertyName);

            var valueAsString = value + "";

            return valueAsString;
        }

        /// <summary>
        ///     Lines to contract.
        /// </summary>
        static TContract LineToContract<TContract>(IReadOnlyList<string> rowCells, IReadOnlyList<string> propertyNames) where TContract : new()
        {
            var contract = new TContract();

            for (var i = 0; i < propertyNames.Count; i++)
            {
                var propertyName = propertyNames[i];
                var cellValue    = rowCells[i];

                var propertyType = Reflection.GetPropertyType(contract, propertyName);

                var propertyValue = Cast.To(cellValue, propertyType, CultureInfo.CurrentCulture);

                Reflection.SetPropertyValue(contract, propertyName, propertyValue);
            }

            return contract;
        }
        #endregion

        /// <summary>
        ///     The reflection
        /// </summary>
        static class Reflection
        {
            #region Public Methods
            /// <summary>
            ///     Gets the type of the property.
            /// </summary>
            public static Type GetPropertyType(object instance, string propertyName)
            {
                var propertyInfo = instance.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new ArgumentException("PropertyNotFound." + propertyName);
                }

                return propertyInfo.PropertyType;
            }

            /// <summary>
            ///     Gets the property value.
            /// </summary>
            public static object GetPropertyValue(object instance, string propertyName)
            {
                var propertyInfo = instance.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new ArgumentException("PropertyNotFound." + propertyName);
                }

                return propertyInfo.GetValue(instance);
            }

            /// <summary>
            ///     Sets the property value.
            /// </summary>
            public static void SetPropertyValue(object instance, string propertyName, object value)
            {
                var propertyInfo = instance.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new ArgumentException("PropertyNotFound." + propertyName);
                }

                propertyInfo.SetValue(instance, value);
            }
            #endregion
        }
    }
}