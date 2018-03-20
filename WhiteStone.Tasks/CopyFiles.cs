using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using WhiteStone.Helpers;

namespace WhiteStone.Tasks
{
    public class CopyFiles : TaskBase
    {
        #region Properties
        string Source => GetKey(nameof(Source));
        string Target => GetKey(nameof(Target));
        #endregion

        #region Public Methods
        public override void Run()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Target).AssertNotNull());

            var filePaths = ParseSource(Source);

            foreach (var filePath in filePaths)
            {
                var destFileName = Target + new FileInfo(filePath).Name;

                File.Copy(filePath, destFileName, true);
            }
        }
        #endregion

        #region Methods
        internal static IEnumerable<string> ParseSource(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            var lastSeperatorIndex = source.LastIndexOf(Path.DirectorySeparatorChar);

            var prefix = source.Substring(0, lastSeperatorIndex) + Path.DirectorySeparatorChar;

            return source.Substring(lastSeperatorIndex + 1).Split('|').Where(x => x.HasValue()).Select(x => prefix + x.Trim()).ToList();
        }
        #endregion
    }
}