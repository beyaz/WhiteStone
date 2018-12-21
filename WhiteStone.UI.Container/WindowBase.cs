using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WhiteStone.UI.Container
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public  class WindowBase : MahApps.Metro.Controls.MetroWindow ,INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public WindowBase()
        {
            Height                = 500;
            Width                 = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Resources.MergedDictionaries.Add(new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")});
            Resources.MergedDictionaries.Add(new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")});
            Resources.MergedDictionaries.Add(new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml")});
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
        #endregion
    }

}