using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace BOA.Office.Excel
{
    /// <summary>
    ///     Defines the excel wrapper.
    /// </summary>
    public class Wrapper : IDisposable
    {
        #region Fields
        /// <summary>
        ///     The excel application
        /// </summary>
        Application _excelApplication;

        /// <summary>
        ///     The workbook
        /// </summary>
        Workbook _workbook;
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the excel application.
        /// </summary>
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

        /// <summary>
        ///     Gets the work book.
        /// </summary>
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

        #region Public Methods
        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            WorkBook.SaveAs(FilePath);

            WorkBook.Close();
        }

        /// <summary>
        ///     Updates the cell.
        /// </summary>
        public void UpdateCell<T>(string sheetName, int rowIndex, int columnIndex, T value)
        {
            var mWorkSheets = WorkBook.Worksheets;
            var mWSheet1    = (Worksheet) mWorkSheets[sheetName];

            mWSheet1.Cells[rowIndex, columnIndex] = value;
        }
        #endregion

        #region Methods
        #region Helper methods
        /// <summary>
        ///     Kills the specific excel file process.
        /// </summary>
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
        #endregion

        #region Dispose
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
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