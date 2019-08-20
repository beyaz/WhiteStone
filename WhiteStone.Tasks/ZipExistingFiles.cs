using System;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    /// <summary>
    ///     The zip existing files data
    /// </summary>
    [Serializable]
    public class ZipExistingFilesData
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the input file paths.
        /// </summary>
        public string[] InputFilePaths { get; set; }

        /// <summary>
        ///     Gets or sets the output file path.
        /// </summary>
        public string OutputFilePath { get; set; }
        #endregion
    }

    /// <summary>
    ///     The zip existing files
    /// </summary>
    public class ZipExistingFiles
    {
        #region Public Methods
        /// <summary>
        ///     Runs the specified data.
        /// </summary>
        public static void Run(ZipExistingFilesData data)
        {
            ZipHelper.CompressFiles(data.OutputFilePath,  data.InputFilePaths);
        }
        #endregion
    }
}