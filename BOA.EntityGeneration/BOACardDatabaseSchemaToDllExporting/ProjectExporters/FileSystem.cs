using BOA.DataFlow;
using BOA.TfsAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public static class FileSystem
    {
        #region Static Fields
        public static IDataConstant<string> CheckinComment                          = DataConstant.Create<string>();
        public static IDataConstant<bool>   IntegrateWithTFSAndCheckInAutomatically = DataConstant.Create<bool>();
        #endregion

        #region Public Methods
        public static  void WriteAllText(IDataContext context, string path, string content)
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