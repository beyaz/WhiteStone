using System;
using System.IO;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class CopyFileData
    {
        #region Public Properties
        public string Source { get; set; }
        public string Destination { get; set; }
        #endregion
    }

    public static class CopyFile
    {
        #region Public Methods
        public static void Run(CopyFileData data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(data.Destination).AssertNotNull());
            File.Copy(data.Source, data.Destination, true);
        }
        #endregion
    }
}