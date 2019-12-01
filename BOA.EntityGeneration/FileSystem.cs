using BOA.Common.Helpers;
using BOA.TfsAccess;
using FileAccess = BOA.TfsAccess.FileAccess;

namespace BOA.EntityGeneration
{
    /// <summary>
    ///     The file system
    /// </summary>
    public sealed class FileSystem
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the checkin comment.
        /// </summary>
        public string CheckinComment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [integrate with TFS and check in automatically].
        /// </summary>
        public bool IntegrateWithTFSAndCheckInAutomatically { get; set; }

        public bool IntegrateWithTFS { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Writes all text.
        /// </summary>
        public void WriteAllText(string path, string content)
        {

            if (IntegrateWithTFS == false && IntegrateWithTFSAndCheckInAutomatically == false)
            {
                FileHelper.WriteAllText(path,content);
                return;
            }

            FileAccessWriteResult fileAccessWriteResult = null;

            if (IntegrateWithTFSAndCheckInAutomatically)
            {
                var fileAccessWithAutoCheckIn = new FileAccessWithAutoCheckIn {CheckInComment = CheckinComment};

                fileAccessWriteResult = fileAccessWithAutoCheckIn.WriteAllText(path, content);
            }
            else
            {
                var fileAccess = new FileAccess();

                fileAccessWriteResult = fileAccess.WriteAllText(path, content);
            }

            if (fileAccessWriteResult.Exception != null)
            {
                throw fileAccessWriteResult.Exception;
            }
        }
        #endregion
    }
}