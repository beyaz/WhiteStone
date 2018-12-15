using System.Windows;
using Notifications.Wpf;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Static Fields
        static readonly NotificationManager notificationManager = new NotificationManager();
        #endregion

        #region Public Methods
        public static void ShowErrorNotification(string message)
        {
            ShowNotification(null, message, NotificationType.Error);
        }

        public static void ShowErrorNotification(string title, string message)
        {
            ShowNotification(title, message, NotificationType.Error);
        }

        public static void ShowSuccessNotification(string message)
        {
            ShowNotification(null, message, NotificationType.Success);
        }

        public static void ShowSuccessNotification(string title, string message)
        {
            ShowNotification(title, message, NotificationType.Success);
        }
        #endregion

        #region Methods
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