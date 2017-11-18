using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    public class GetFileContentFromWebIfNotInTarggetDir : ITask
    {
        #region Public Properties
        public IDictionary<string, string> Keys { get; set; }
        #endregion

        #region Properties
        string SourceUrl => Keys[nameof(SourceUrl)];
        string TargetDirectory => Keys[nameof(TargetDirectory)];
        #endregion

        #region Public Methods
        public void Run()
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