using System.Collections.Generic;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    public class CopyDirectory : ITask
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
            FileHelper.CopyDirectory(Source, Target, true);
        }
        #endregion
    }
}