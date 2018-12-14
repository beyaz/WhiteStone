using System.Configuration;
using System.Diagnostics;
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
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }
        #endregion

        #region Public Methods
        public static void EmptyStart()
        {
            var builder = new BOA.Jaml.Builder();

            var ui = @"
{
    view:'Grid',
	cols:
    [
		{view:'TextArea', Gravity:2 },
        {view:'GridSplitter'},
        {view:'TextBox', Gravity:4 }
	]
}
";
            Debug.Assert(Application.Current.MainWindow != null, "Application.Current.MainWindow != null");

            Application.Current.MainWindow.Content = builder.SetJson(ui).Build().View;
        }
        #endregion

        #region Methods
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