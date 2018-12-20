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



        #region bool BooleanProperty1
        bool _booleanProperty1;

        public bool BooleanProperty1
        {
            get { return _booleanProperty1; }
            set
            {
                if (_booleanProperty1 != value)
                {
                    _booleanProperty1 = value;
                    OnPropertyChanged();
                }
            }
        }



        #endregion
    }
}