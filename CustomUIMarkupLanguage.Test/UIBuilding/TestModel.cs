using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomUIMarkupLanguage.Test.UIBuilding
{
    [Serializable]
    public class TestModel : INotifyPropertyChanged
    {
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region UserModel User
        UserModel _user;

        public UserModel User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}