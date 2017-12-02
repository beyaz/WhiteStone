using System;
using System.IO;

namespace WhiteStone.Tasks
{
    public class CopyFile : TaskBase
    {
        #region Properties
        string Source => GetKey(nameof(Source));
        string Target => GetKey(nameof(Target));
        #endregion

        #region Public Methods
        public override void Run()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Target).AssertNotNull());
            File.Copy(Source, Target, true);
        }
        #endregion
    }
}