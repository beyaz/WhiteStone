using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace BOA.Office.Excel
{
    /// <summary>
    ///     The reader
    /// </summary>
    public static class Reader
    {
        #region Public Methods
        /// <summary>
        ///     Read excel file by using oledb connection.
        /// </summary>
        public static DataTable ReadFromFile(string fileName, string sheetName)
        {
            #region Argument validations
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentNullException("sheetName");
            }
            #endregion

            string connectionString;
            var    fileExtension = Path.GetExtension(fileName);
            var dataSet = new DataSet
            {
                Locale = CultureInfo.InvariantCulture
            };

            if (fileExtension == ".xls")
            {
                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + ";Extended Properties=Excel 8.0;";
            }
            else
            {
                if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + fileName + ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    throw new ArgumentException("Dosya uzantısı .xls veya .xlsx olmalıdır.");
                }
            }

            var excelSheetName = sheetName + "$";

            var dataAdapter = new OleDbDataAdapter("SELECT * FROM [" + excelSheetName + "] ", connectionString);
            dataAdapter.Fill(dataSet, "ExcelInfo");
            return dataSet.Tables["ExcelInfo"];
        }
        #endregion
    }
}