using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace WhiteStone.Office
{
    /// <summary>
    /// Defines the excel wrapper.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ExcelWrapper : IDisposable
    {
        /// <summary>
        /// Updates the cell.
        /// </summary>
        public void UpdateCell<T>(string sheetName, int rowIndex, int columnIndex, T value)
        {
            var mWorkSheets = WorkBook.Worksheets;
            var mWSheet1 = (Worksheet) mWorkSheets[sheetName];

            mWSheet1.Cells[rowIndex, columnIndex].Value2 = value;
        }
        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            WorkBook.SaveAs(FilePath);

            WorkBook.Close();
        }

        #region Helper methods

        static void KillSpecificExcelFileProcess(string excelFileName)
        {
            var processes = from p in Process.GetProcessesByName("EXCEL")
                select p;

            foreach (var process in processes)
            {
                if (process.MainWindowTitle == "Microsoft Excel - " + excelFileName)
                {
                    process.Kill();
                }
            }
        }

        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        Application _excelApplication;

        Application ExcelApplication
        {
            get
            {
                if (_excelApplication == null)
                {
                    _excelApplication = new Application();
                }
                return _excelApplication;
            }
        }

        Workbook _workbook;

        Workbook WorkBook
        {
            get
            {
                if (FilePath == null)
                {
                    throw new ArgumentException("FilePath");
                }
                if (_workbook == null)
                {
                    var processName = Path.GetFileName(FilePath);

                    KillSpecificExcelFileProcess(processName);

                    _workbook = ExcelApplication.Workbooks.Open(FilePath);
                }

                return _workbook;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        #endregion
    }
}