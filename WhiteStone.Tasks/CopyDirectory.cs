using System;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class CopyDirectoryData
    {
        #region Public Properties
        public string Source { get; set; }
        public string Destination { get; set; }
        #endregion
    }

    public class CopyDirectory 
    {
      

        #region Public Methods
        public static void Run(CopyDirectoryData data)
        {
            FileHelper.CopyDirectory(data.Source, data.Destination, true);
        }
        #endregion
    }
}