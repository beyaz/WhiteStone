using System.Configuration;
using System.Windows;
using BOA.Common.Helpers;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Called when [main window loaded].
        /// </summary>
        void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var startMethod = ConfigurationManager.AppSettings[nameof(OnMainWindowLoaded)];
            if (string.IsNullOrWhiteSpace(startMethod))
            {
                Log.IsNull(nameof(OnMainWindowLoaded));
                return;
            }

            var mi = ReflectionHelper.GetMethod(startMethod);
            if (mi == null)
            {
                Log.IsNull(nameof(mi));
                return;
            }

            mi.Invoke(null, null);
        }
        #endregion
    }
}