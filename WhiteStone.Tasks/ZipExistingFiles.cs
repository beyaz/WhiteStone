using System;
using System.Collections.Generic;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    /// <summary>
    ///     The zip existing files data
    /// </summary>
    [Serializable]
    public class ZipWhiteStoneProjectData
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

    static class FilePathHelper
    {
        #region Static Fields
        static readonly Dictionary<string, string> ReplaceMap = new Dictionary<string, string>
        {
            {"$(SolutionDir)", "D:\\github\\WhiteStone\\"}
        };
        #endregion

        #region Public Methods
        public static string Normalize(string path)
        {
            foreach (var pair in ReplaceMap)
            {
                path = path.Replace(pair.Key, pair.Value);
            }

            return path;
        }
        #endregion
    }

    /// <summary>
    ///     The zip existing files
    /// </summary>
    public class ZipWhiteStoneProject
    {
        #region Public Methods
        /// <summary>
        ///     Runs the specified data.
        /// </summary>
        public static void Run(ZipWhiteStoneProjectData data)
        {
            for (var i = 0; i < data.InputFilePaths.Length; i++)
            {
                data.InputFilePaths[i] = FilePathHelper.Normalize(data.InputFilePaths[i]);
            }

            data.OutputFilePath = FilePathHelper.Normalize(data.OutputFilePath);

            ZipHelper.CompressFiles(data.OutputFilePath, data.InputFilePaths);
        }
        #endregion
    }

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

            for (var i = 0; i < data.InputFilePaths.Length; i++)
            {
                data.InputFilePaths[i] = FilePathHelper.Normalize(data.InputFilePaths[i]);
            }

            data.OutputFilePath = FilePathHelper.Normalize(data.OutputFilePath);

            ZipHelper.CompressFiles(data.OutputFilePath, data.InputFilePaths);
        }
        #endregion
    }
}