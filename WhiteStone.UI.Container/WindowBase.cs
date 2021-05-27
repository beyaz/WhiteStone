using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage;
using MahApps.Metro.Controls;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public class WindowBase : MetroWindow, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowBase" /> class.
        /// </summary>
        public WindowBase()
        {
            Height                = 500;
            Width                 = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ApplyMahAppMetroStyle();

            Closed += KillAllContainer;
        }
        #endregion

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        static void KillAllContainer(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcesses())
            {
                var name = typeof(WindowBase).Assembly.GetName().Name;
                if (process.ProcessName == name)
                {
                    process.Kill();
                }
            }
        }

        void ApplyMahAppMetroStyle()
        {
            var useMahAppMetroStyle = ConfigHelper.GetFromAppSetting("UseMahAppMetroStyle").To<bool?>() ?? true;
            if (useMahAppMetroStyle)
            {
                MahAppHelper.MergeMahAppMetroStyles(Resources);
            }
        }
        #endregion
    }
}