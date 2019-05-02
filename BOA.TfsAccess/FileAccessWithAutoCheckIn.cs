using BOA.CodeGeneration.Util;

namespace BOA.TfsAccess
{
    public class FileAccessWithAutoCheckIn : FileAccess
    {
        #region Public Methods
        public override FileAccessWriteResult WriteAllText(string path, string content)
        {
            var result = base.WriteAllText(path, content);

            if (result.FileIsCheckOutFromTfs)
            {
                TFSAccessForBOA.CheckInFile(path, "2235# - AutoCheckIn");
            }

            return result;
        }
        #endregion
    }
}