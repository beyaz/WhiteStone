using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace WhiteStone.Office
{
    /// <summary>
    ///     Reads excel file
    /// </summary>
    public static class ExcelReader
    {
        /// <summary>
        ///     Read excel file by using oledb connection.
        ///     <exception cref="ArgumentNullException">When fileName or sheetName is empty</exception>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
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
            var fileExtension = Path.GetExtension(fileName);
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
    }
}