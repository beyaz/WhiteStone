using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class CopyFilesData
    {
        #region Public Properties
        public string Source { get; set; }
        public string Destination { get; set; }
        #endregion
    }

    public static class CopyFiles
    {
        #region Public Methods
        public static void Run(CopyFilesData data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(data.Destination).AssertNotNull());

            var filePaths = ParseSource(data.Source);

            foreach (var filePath in filePaths)
            {
                var destFileName = data.Destination + new FileInfo(filePath).Name;

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

            var lastSeparatorIndex = source.LastIndexOf(Path.DirectorySeparatorChar);

            var prefix = source.Substring(0, lastSeparatorIndex) + Path.DirectorySeparatorChar;

            return source.Substring(lastSeparatorIndex + 1).Split('|').Where(x => x.HasValue()).Select(x => prefix + x.Trim()).ToList();
        }
        #endregion
    }
}