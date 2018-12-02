using System.Windows;
using System.Windows.Navigation;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Model = new MainWindowModel();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SlnFileSelection.Model = Model;
            TableSelection.Model = Model;
        }
        #endregion

        #region Methods
        void HideAllWizards()
        {
            SlnFileSelection.Visibility = Visibility.Collapsed;
        }

        void OnNextButtonClick(object sender, RoutedEventArgs e)
        {
            HideAllWizards();

            if (!SlnFileSelection.IsOk)
            {
                SlnFileSelection.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region MainWindowModel Model
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(MainWindowModel), typeof(MainWindow));

        public MainWindowModel Model
        {
            get { return (MainWindowModel) GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        #endregion
    }
}