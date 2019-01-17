using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.UIBuilding;
using MahApps.Metro.Controls;
using Notifications.Wpf;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public class App : Application
    {
        #region Static Fields
        /// <summary>
        ///     The notification manager
        /// </summary>
        static readonly NotificationManager notificationManager = new NotificationManager();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Handles the exception.
        /// </summary>
        public static void HandleException(Exception exception)
        {
            var messages = new List<string>();
            while (true)
            {
                if (exception == null)
                {
                    break;
                }

                messages.Add(exception.Message);

                exception = exception.InnerException;
            }

            ShowErrorNotification(string.Join(Environment.NewLine, messages));
        }

        /// <summary>
        ///     Initializes the builder.
        /// </summary>
        public static void InitializeBuilder()
        {
            Builder.RegisterElementCreation(IntellisenseTextBox.On);

            Builder.RegisterElementCreation(LabeledTextBox.On);
            Builder.RegisterElementCreation(LabeledComboBox.On);

            Builder.RegisterElementCreation("Tile", typeof(Tile));
        }

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
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

        /// <summary>
        ///     Startups the main window.
        /// </summary>
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
        #endregion
    }
}