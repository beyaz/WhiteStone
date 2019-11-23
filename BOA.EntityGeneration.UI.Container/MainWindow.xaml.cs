namespace BOA.EntityGeneration.UI.Container
{
    public partial class MainWindow
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.Model;
        }
        #endregion
    }
}