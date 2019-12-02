namespace BOA.EntityGeneration.UI.Container
{
    public partial class App
    {
        #region Static Fields
        public static readonly MainWindowModel Model;

        internal static readonly EntityGenerationUIContainerConfig Config = EntityGenerationUIContainerConfig.CreateFromFile();
        #endregion

        #region Constructors
        static App()
        {
            Model = new MainWindowModel
            {
                CheckinComment = CheckInCommentAccess.GetCheckInComment()
            };
        }
        #endregion
    }
}