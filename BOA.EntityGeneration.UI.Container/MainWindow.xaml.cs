using static BOA.EntityGeneration.UI.Container.App;
using static BOA.EntityGeneration.UI.Container.Data;

namespace BOA.EntityGeneration.UI.Container
{
    public partial class MainWindow
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Model[Context];
        }
        #endregion
    }
}