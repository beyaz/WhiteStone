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

    /// <summary>
    ///     The zip existing files
    /// </summary>
    public class ZipWhiteStoneProject
    {

        static readonly Dictionary<string, string> ReplaceMap = new Dictionary<string, string>
        {
            {"$(SolutionDir)", "D:\\github\\WhiteStone\\"}
        };

        #region Public Methods
        /// <summary>
        ///     Runs the specified data.
        /// </summary>
        public static void Run(ZipWhiteStoneProjectData data)
        {
            for (var i = 0; i < data.InputFilePaths.Length; i++)
            {
                foreach (var pair in ReplaceMap)
                {
                    data.InputFilePaths[i] =  data.InputFilePaths[i].Replace(pair.Key,pair.Value);    
                }
            }

            foreach (var pair in ReplaceMap)
            {
               data.OutputFilePath = data.OutputFilePath.Replace(pair.Key,pair.Value);    
            }

            ZipHelper.CompressFiles(data.OutputFilePath,  data.InputFilePaths);
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
            ZipHelper.CompressFiles(data.OutputFilePath,  data.InputFilePaths);
        }
        #endregion
    }
}