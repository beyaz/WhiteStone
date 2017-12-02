using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    public class CopyDirectory : TaskBase
    {
        #region Properties
        string Source => GetKey(nameof(Source));
        string Target => GetKey(nameof(Target));
        #endregion

        #region Public Methods
        public override void Run()
        {
            FileHelper.CopyDirectory(Source, Target, true);
        }
        #endregion
    }
}