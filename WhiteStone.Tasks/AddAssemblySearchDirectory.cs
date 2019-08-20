using System;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{

    [Serializable]
    public class AddAssemblySearchDirectoryData
    {
        #region Public Properties
        public string DirectoryPath { get; set; }
        #endregion
    }

    public static class AddAssemblySearchDirectory
    {
        #region Public Methods
        public static void Run(AddAssemblySearchDirectoryData data)
        {
            AppDomain.CurrentDomain.AddAssemblySearchDirectory(data.DirectoryPath);
        }
        #endregion
    }
}