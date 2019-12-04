namespace BOA.TfsAccess
{
    public class FileAccessWithAutoCheckIn : FileAccess
    {
        #region Public Properties
        public string CheckInComment { get; set; } = "2235# - AutoCheckIn";
        #endregion

        #region Public Methods
        public override FileAccessWriteResult WriteAllText(string path, string content)
        {
            var result = base.WriteAllText(path, content);

            if (result.FileIsCheckOutFromTfs)
            {
                TFSAccessForBOA.CheckInFile(path, CheckInComment);
            }

            return result;
        }
        #endregion
    }
}