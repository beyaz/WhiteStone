using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Jaml
{
    [Serializable]
    public class UserModel : INotifyPropertyChanged
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

        #region string UserName
        string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region string Password
        string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }

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

    [TestClass]
    public class BuilderTest : DependencyObject
    {
        #region Static Fields
        public static readonly DependencyProperty TestPropertyNullableInt32Property = DependencyProperty.Register(
                                                                                                                  "TestPropertyNullableInt32", typeof(int?), typeof(BuilderTest), new PropertyMetadata(default(int?)));
        #endregion

        #region Public Methods
        [TestMethod]
        public void GetBinding()
        {
            var typeFinder = new TypeFinder();
            var expression = "{Binding Path=Model.Customers, Mode=OneWay,Converter =" + typeof(ConverterClass).FullName + "}";
            var binding    = expression.ConvertToBinding(typeFinder);
            Assert.IsTrue(binding.Mode == BindingMode.OneWay);
            Assert.IsTrue(binding.Path.Path == "Model.Customers");
            Assert.IsTrue(binding.Converter is ConverterClass);

            var text = "{Binding Model.Contract.SaleAmount}";

            binding = text.ConvertToBinding(typeFinder);
            Assert.IsTrue(binding.Path.Path == "Model.Contract.SaleAmount");

            text = " { Model.Contract.SaleAmount } ";

            binding = text.ConvertToBinding(typeFinder);
            Assert.IsTrue(binding.Path.Path == "Model.Contract.SaleAmount");

            text    = " { Model.Contract.SaleAmount, UpdateSourceTrigger=PropertyChanged } ";
            binding = text.ConvertToBinding(typeFinder);
            Assert.IsTrue(binding.UpdateSourceTrigger == UpdateSourceTrigger.PropertyChanged);
        }

        [TestMethod]
        public void Grid_Cols()
        {
            var view = new Builder().SetJson(@"
{
    view:'Grid',
	cols:
    [
		{view:'TextBox', Gravity:2 },
        {view:'TextBox', Gravity:3 },
        {view:'TextBox', Gravity:4 }
	]
}
").Build().View;

            Assert.IsTrue(view is Grid);
            Assert.IsTrue(((Grid) view).ColumnDefinitions.Count == 3);
            Assert.IsTrue(((Grid) view).ColumnDefinitions[2].Width.Value + "" == "4");
        }

        [TestMethod]
        public void Grid_Rows()
        {
            var view = new Builder().SetJson(@"
{
    view:'Grid',
	rows:
    [
		{view:'TextBox'},
        {view:'TextBox'},
        {view:'TextBox'}
	]
}
").Build().View;

            Assert.IsTrue(view is Grid);
            Assert.IsTrue(((Grid) view).RowDefinitions.Count == 3);
        }

        [TestMethod]
        public void SearchDependencyProperty()
        {
            Assert.IsNotNull(BuilderUtility.SearchDependencyProperty("System.Windows.FrameworkElement.WidthProperty", new TypeFinder()));
            Assert.IsNotNull(BuilderUtility.SearchDependencyProperty(GetType().FullName + ".TestPropertyNullableInt32Property", new TypeFinder()));
        }

        [TestMethod]
        public void StackPanel()
        {
            var user = new UserModel
            {
                UserName = "Aloha"
            };

            var model = new TestModel
            {
                User = user
            };

            var view = new Builder
            {
                DataContext = model
            }.SetJson(@"
{
    view:'StackPanel',
	Children:[
		{view:'TextBox', Text:'{Binding User.UserName}'},
        {view:'TextBox', Text:'{Binding User.Password}'},
        {view:'Label',   Content:'aloha',Width:'56.9',HorizontalAlignment:'Center'},
        {view:'Label',   Content:'{Binding Model.Contract.Password}'}

	]
}
").Build().View;

            Assert.IsTrue(view is StackPanel);

            Assert.AreEqual(4, ((StackPanel) view).Children.Count);

            var userNameTextBox = (TextBox) ((StackPanel) view).Children[0];

            Assert.IsTrue(user.UserName == userNameTextBox.Text);

            user.UserName = "yyy";
            Assert.IsTrue(user.UserName == userNameTextBox.Text);
        }
        #endregion
    }

    public class ConverterClass : IValueConverter
    {
        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}