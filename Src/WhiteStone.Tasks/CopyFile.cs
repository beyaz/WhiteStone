using System.Collections.Generic;
using System.IO;

namespace WhiteStone.Tasks
{
    public class CopyFile : ITask
    {
        #region Public Properties
        public IDictionary<string, string> Keys { get; set; }
        #endregion

        #region Properties
        string Source => Keys[nameof(Source)];
        string Target => Keys[nameof(Target)];
        #endregion

        #region Public Methods
        public void Run()
        {
            File.Copy(Source, Target, true);
        }
        #endregion
    }
}