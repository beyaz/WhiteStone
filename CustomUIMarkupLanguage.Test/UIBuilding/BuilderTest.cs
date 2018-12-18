using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CustomUIMarkupLanguage.UIBuilding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomUIMarkupLanguage.Test.UIBuilding
{
    [TestClass]
    public class BuilderTest : DependencyObject
    {
        #region Static Fields
        public static readonly DependencyProperty TestPropertyNullableInt32Property = DependencyProperty.Register("TestPropertyNullableInt32", typeof(int?), typeof(BuilderTest), new PropertyMetadata(default(int?)));
        #endregion

        #region Public Methods
        [TestMethod]
        public void GetBinding()
        {
            var typeFinder = new TypeFinder();
            var expression = "{Binding Model.Customers, Mode=OneWay,Converter =" + typeof(ConverterClass).FullName + "}";
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
        }

        [TestMethod]
        public void SearchDependencyProperty()
        {
            Assert.IsNotNull(BuilderUtility.SearchDependencyProperty("System.Windows.FrameworkElement.WidthProperty", new TypeFinder()));
            Assert.IsNotNull(BuilderUtility.SearchDependencyProperty(GetType().FullName + ".TestPropertyNullableInt32Property", new TypeFinder()));
        }

        [TestMethod]
        public void Test1()
        {
            var textBox = new TextBox();

            const string ui = "{Text:'A'}";

            var builder = new Builder
            {
                Caller = textBox
            };

            builder.Load(ui);

            Assert.AreEqual("A", textBox.Text);
        }

        [TestMethod]
        public void Test2_GridCols()
        {
            var view = new Grid();

            const string ui = @"
{
    view:'Grid',
	cols:
    [
		{view:'TextBox', Gravity:2 },
        {view:'TextBox', Gravity:3 },
        {view:'TextBox', Gravity:4 }
	]
}
";
            var builder = new Builder
            {
                Caller = view
            };
            builder.Load(ui);

            Assert.IsTrue(view.ColumnDefinitions.Count == 3);
            Assert.IsTrue(view.ColumnDefinitions[2].Width.Value + "" == "4");
        }

        [TestMethod]
        public void Test3_GridCols()
        {
            var view = new Grid();

            const string ui = @"
{
    view:'Grid',
	rows:
    [
		{view:'TextBox'},
        {view:'TextBox'},
        {view:'TextBox'}
	]
}
";
            var builder = new Builder
            {
                Caller = view
            };
            builder.Load(ui);

            Assert.IsTrue(view.RowDefinitions.Count == 3);
        }

        [TestMethod]
        public void Test4_StackPanel_With_Binding()
        {
            var user = new UserModel
            {
                UserName = "Aloha"
            };

            var model = new TestModel
            {
                User = user
            };

            var view = new StackPanel();

            const string ui = @"
{
    view:'StackPanel',
	Children:[
		{view:'TextBox', Text:'{Binding User.UserName}'},
        {view:'TextBox', Text:'{Binding User.Password}'},
        {view:'Label',   Content:'aloha',Width:'56.9',HorizontalAlignment:'Center'},
        {view:'Label',   Content:'{Binding Model.Contract.Password}'}

	]
}
";
            var builder = new Builder
            {
                Caller      = view,
                DataContext = model
            };
            builder.Load(ui);

            Assert.AreEqual(4, view.Children.Count);

            var userNameTextBox = (TextBox) view.Children[0];

            Assert.IsTrue(user.UserName == userNameTextBox.Text);

            user.UserName = "yyy";
            Assert.IsTrue(user.UserName == userNameTextBox.Text);
        }
        #endregion
    }
}