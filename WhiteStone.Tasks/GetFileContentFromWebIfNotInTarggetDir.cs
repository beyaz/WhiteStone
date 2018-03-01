using System;
using System.IO;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    public class GetFileContentFromWebIfNotInTarggetDir : TaskBase
    {
        #region Properties
        string SourceUrl => GetKey(nameof(SourceUrl));
        string TargetDirectory => GetKey(nameof(TargetDirectory));
        #endregion

        #region Public Methods
        public override void Run()
        {
            var fileName = Path.GetFileName(SourceUrl);

            var targetFile = Path.Combine(TargetDirectory, fileName.AssertNotNull());
            if (!File.Exists(targetFile))
            {
                var content = FileHelper.DownloadString(SourceUrl);

                FileHelper.WriteAllText(targetFile, content);
            }
        }
        #endregion
    }
}