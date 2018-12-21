using System.Configuration;
using System.Windows;
using BOA.Common.Helpers;

namespace WhiteStone.UI.Container
{
    class MainWindow : WindowBase
    {
        public MainWindow()
        {
            Loaded += OnMainWindowLoaded;
        }
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

    }
}