using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using BOA.Common.Helpers;
using MahApps.Metro.Controls;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public class WindowBase : MetroWindow, INotifyPropertyChanged
    {
        readonly  bool UseMahAppMetroStyle;
        #region Static Fields
        static readonly List<ResourceDictionary> MahAppsResourceDictionaries;
        #endregion

        #region Constructors
        static WindowBase()
        {
            MahAppsResourceDictionaries = new List<ResourceDictionary>
            {
                new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")},
                new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")},
                new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml")}
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowBase" /> class.
        /// </summary>
        public WindowBase()
        {
            Height                = 500;
            Width                 = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            UseMahAppMetroStyle = ConfigHelper.GetFromAppSetting(nameof(UseMahAppMetroStyle)).To<bool?>() ?? true;

            if (UseMahAppMetroStyle)
            {
                foreach (var dictionary in MahAppsResourceDictionaries)
                {
                    Resources.MergedDictionaries.Add(dictionary);
                }
            }

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

        void KillAllContainer(object sender, EventArgs e)
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
        #endregion
    }
}