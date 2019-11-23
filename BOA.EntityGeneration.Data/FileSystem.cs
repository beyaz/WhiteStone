using BOA.DataFlow;
using BOA.TfsAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public static class FileSystem
    {
        #region Static Fields
        public static IProperty<string> CheckinComment                          = Property.Create<string>();
        public static IProperty<bool>   IntegrateWithTFSAndCheckInAutomatically = Property.Create<bool>();
        #endregion

        #region Public Methods
        public static void WriteAllText(IContext context, string path, string content)
        {
            FileAccessWriteResult fileAccessWriteResult = null;

            if (context.TryGet(IntegrateWithTFSAndCheckInAutomatically))
            {
                var fileAccessWithAutoCheckIn = new FileAccessWithAutoCheckIn {CheckInComment = context.Get(CheckinComment)};

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