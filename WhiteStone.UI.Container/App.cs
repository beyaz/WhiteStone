using System;
using System.Configuration;
using System.Windows;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.UIBuilding;
using Notifications.Wpf;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App:System.Windows.Application
    {

          public static void InitializeBuilder()
        {
            Builder.RegisterElementCreation(IntellisenseTextBox.On);

            Builder.RegisterElementCreation(LabeledTextBox.On);
            

            Builder.RegisterElementCreation("Tile",typeof(MahApps.Metro.Controls.Tile));
        }

        static Window StartupMainWindow()
        {
            var startupMainWindow = ConfigurationManager.AppSettings[nameof(StartupMainWindow)];
            if (string.IsNullOrWhiteSpace(startupMainWindow))
            {
                Log.IsNull(nameof(StartupMainWindow));
                return null;
            }

            var type = Type.GetType(startupMainWindow);
            if (type == null)
            {
                Log.IsNull(startupMainWindow);
                throw new ArgumentNullException(nameof(type));
            }

            return (Window) Activator.CreateInstance(type);

        }

        [STAThread]
        public static void Main()
        {
            InitializeBuilder();

            var application = new App
            {
                MainWindow = StartupMainWindow()
            };
            application.MainWindow.Show();
            application.Run();
        }

        #region Static Fields
        /// <summary>
        ///     The notification manager
        /// </summary>
        static readonly NotificationManager notificationManager = new NotificationManager();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Shows the error notification.
        /// </summary>
        public static void ShowErrorNotification(string message)
        {
            ShowNotification(null, message, NotificationType.Error);
        }

        /// <summary>
        ///     Shows the error notification.
        /// </summary>
        public static void ShowErrorNotification(string title, string message)
        {
            ShowNotification(title, message, NotificationType.Error);
        }

        /// <summary>
        ///     Shows the success notification.
        /// </summary>
        public static void ShowSuccessNotification(string message)
        {
            ShowNotification(null, message, NotificationType.Success);
        }

        /// <summary>
        ///     Shows the success notification.
        /// </summary>
        public static void ShowSuccessNotification(string title, string message)
        {
            ShowNotification(title, message, NotificationType.Success);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Shows the notification.
        /// </summary>
        static void ShowNotification(string title, string message, NotificationType notificationType)
        {
            notificationManager.Show(new NotificationContent
            {
                Title   = title,
                Message = message,
                Type    = notificationType
            });
        }
        #endregion
    }
}