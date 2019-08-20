using System;
using BOA.CodeGeneration.Util;

namespace BOA.TfsAccess.Tasks
{
    public class CheckinFile
    {
        #region Public Methods
        public static void Run(CheckinFileData data)
        {
            TFSAccessForBOA.CheckInFile(data.Path, data.Comment);
        }
        #endregion
    }

    [Serializable]
    public sealed class CheckinFileData
    {
        #region Public Properties
        public string Comment { get; set; }
        public string Path    { get; set; }
        #endregion
    }
}