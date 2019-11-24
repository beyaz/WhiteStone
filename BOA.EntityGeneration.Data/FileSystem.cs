using BOA.TfsAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
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
        #endregion

        #region Public Methods
        /// <summary>
        ///     Writes all text.
        /// </summary>
        public void WriteAllText(string path, string content)
        {
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