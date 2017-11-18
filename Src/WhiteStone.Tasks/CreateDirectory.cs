using System.Collections.Generic;
using System.IO;

namespace WhiteStone.Tasks
{
    public class CreateDirectory : ITask
    {
        #region Public Properties
        public IDictionary<string, string> Keys { get; set; }
        #endregion

        #region Properties
        string Path => Keys[nameof(Path)];
        #endregion

        #region Public Methods
        public void Run()
        {
            Directory.CreateDirectory(Path);
        }
        #endregion
    }
}