using System;
using System.Collections.Generic;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    /// <summary>
    ///     The un zip data
    /// </summary>
    [Serializable]
    public class UnZipData
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the file map.
        /// </summary>
        public IReadOnlyDictionary<string, string> FileMap { get; set; }

        /// <summary>
        ///     Gets or sets the zip file path.
        /// </summary>
        public string ZipFilePath { get; set; }
        #endregion
    }

    /// <summary>
    ///     The un zip
    /// </summary>
    public class UnZip
    {
        #region Public Methods
        /// <summary>
        ///     Runs the specified data.
        /// </summary>
        public static void Run(UnZipData data)
        {
            ZipHelper.ExtractFromZipFile(data.ZipFilePath, null, data.FileMap);
        }
        #endregion
    }
}