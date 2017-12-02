using System.IO;

namespace WhiteStone.Tasks
{
    public class CreateDirectory : TaskBase
    {
        #region Properties
        string Path => GetKey(nameof(Path));
        #endregion

        #region Public Methods
        public override void Run()
        {
            Directory.CreateDirectory(Path);
        }
        #endregion
    }
}