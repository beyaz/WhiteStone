using System;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class ZipExistingFilesData
    {

        #region Public Properties
        public string OutputFilePath { get; set; }
        public string[] InputFilePaths { get; set; }
        #endregion
    }

    public class ZipExistingFiles
    {
        #region Public Methods
        public static void Run(ZipExistingFilesData data)
        {
            ZipHelper.CompressFiles(data.OutputFilePath,null,data.InputFilePaths);
        }
        #endregion
    }
}