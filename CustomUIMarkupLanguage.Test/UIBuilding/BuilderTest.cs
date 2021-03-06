﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using CustomUIMarkupLanguage.Markup;
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
        public void FireEventWithPrimitiveParameters()
        {
            var button = new ExtendedButton2();

            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            Assert.AreEqual("A", (string) button.Content);

            Assert.IsTrue(button.IsClicked);
        }

        #region BindDataContext
        class BindDataContextTestModel
        {
            public BindDataContextTestModel Inner { get; set; }

            public string A { get; set; }
        }

        class BindDataContextTestUI : Grid
        {
            public BindDataContextTestUI()
            {
                this.LoadJson(@"
{
 rows:[
    {ui:'Button',Content:'{Binding A}'},
    {ui:'Button',Content:'{Binding Inner.A}'},
    {ui:'Button',DataContext:'{Binding Inner}', Content:'{Binding A}'}
 ]
}

");
            }
        }


        [TestMethod]
        public void BindDataContext()
        {
            var model = new BindDataContextTestModel
            {
                Inner = new BindDataContextTestModel
                {
                    A = "A1"
                },
                A = "A0"
            };

            var ui = new BindDataContextTestUI
            {
                DataContext = model
            };

            Assert.IsTrue(((ui.Children[0] as Button)?.Content as string) == "A0");
            Assert.IsTrue(((ui.Children[1] as Button)?.Content as string) == "A1");
            Assert.IsTrue(((ui.Children[2] as Button)?.Content as string) == "A0");

        } 
        #endregion


        [TestMethod]
        public void FireEventWithNoParameter()
        {

            var radioButton = new ExtendedRadioButton();

            radioButton.RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));


            Assert.AreEqual("A", radioButton.Value);

            // Assert.IsTrue(radioButton.IsChecked == true);
        }

        class ExtendedRadioButton:RadioButton
        {
            public string Value { get; set; }

            public void OnCheckedChanged()
            {
                Value = "A";
            }

            public ExtendedRadioButton()
            {
                this.LoadJson("{Checked:'OnCheckedChanged'}");
            }
        }
        

        


        [TestMethod]
        public void GetBinding()
        {

            


            

            Func<string,Binding> ParseBinding = (bindingExpressionAsText) => { return BindingExpressionParser.TryParse(bindingExpressionAsText).ConvertToBinding(TypeFinder.GetType, null); };


            var expression = "{Binding Model.Customers, Mode=OneWay,Converter =" + typeof(ConverterClass).FullName + "}";
            var binding    = ParseBinding(expression);
            Assert.IsTrue(binding.Mode == BindingMode.OneWay);
            Assert.IsTrue(binding.Path.Path == "Model.Customers");
            Assert.IsTrue(binding.Converter is ConverterClass);

            var text = "{Binding Model.Contract.SaleAmount}";

            binding = ParseBinding(text);
            Assert.IsTrue(binding.Path.Path == "Model.Contract.SaleAmount");

            text = " { Model.Contract.SaleAmount } ";

            binding = ParseBinding(text);
            Assert.IsTrue(binding.Path.Path == "Model.Contract.SaleAmount");
        }

        [TestMethod]
        public void OnClick()
        {
            var button = new ExtendedButton();

            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            Assert.AreEqual("A", (string) button.Content);

            Assert.IsTrue(button.IsClicked);
        }

        [TestMethod]
        public void SearchDependencyProperty()
        {
            Assert.IsNotNull(BuilderUtility.SearchDependencyProperty("System.Windows.FrameworkElement.WidthProperty", TypeFinder.GetType));
            Assert.IsNotNull(BuilderUtility.SearchDependencyProperty(GetType().FullName + ".TestPropertyNullableInt32Property", TypeFinder.GetType));
        }

        [TestMethod]
        public void ShouldSupportLowerCaseProperties()
        {
            var textBox = new TextBox();

            const string ui = "{text:'A',IsVisible:'{Binding " + nameof(TestModel.BooleanProperty1) + "}'}";

            var dataContext = new TestModel();

            var builder = new Builder
            {
                Caller      = textBox
            };

            builder.Load(ui);

            textBox.DataContext = dataContext;

            Assert.AreEqual("A", textBox.Text);
            Assert.AreEqual(Visibility.Collapsed, textBox.Visibility);

            dataContext.BooleanProperty1 = true;

            Assert.AreEqual(Visibility.Visible, textBox.Visibility);
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
            };
            builder.Load(ui);

            view.DataContext = model;

            Assert.AreEqual(4, view.Children.Count);

            var userNameTextBox = (TextBox) view.Children[0];

            Assert.IsTrue(user.UserName == userNameTextBox.Text);

            user.UserName = "yyy";
            Assert.IsTrue(user.UserName == userNameTextBox.Text);
        }

        [TestMethod]
        public void WindowContentMustBeSupport()
        {
            var window = new Window();

            var builder = new Builder
            {
                Caller = window
            };

            builder.Load("{Content:{ui:'Grid',rows:[{ui:'TextBox',Text:'A'}]}}");
            var grid = (Grid) window.Content;
            Assert.IsTrue(((TextBox)grid.Children[0]).Text == "A" );

        }
        #endregion

        class ExtendedButton : Button
        {
            #region Constructors
            public ExtendedButton()
            {
                const string ui = "{Text:'A', Click:'Click1'}";

                var builder = new Builder
                {
                    Caller = this
                };

                builder.Load(ui);
            }
            #endregion

            #region Public Properties
            public bool IsClicked { get; set; }
            #endregion

            #region Public Methods
            public void Click1(object sender, RoutedEventArgs e)
            {
                if (!Equals(sender, this))
                {
                    throw new ArgumentException();
                }

                IsClicked = true;
            }
            #endregion
        }

        class ExtendedButton2 : Button
        {
            #region Constructors
            public ExtendedButton2()
            {
                const string ui = "{Text:'A', Click:'this.MyClick(2)', Height:34 }";

                var builder = new Builder
                {
                    Caller = this
                };

                builder.Load(ui);
            }
            #endregion

            #region Public Properties
            public bool IsClicked { get; set; }
            #endregion

            #region Public Methods
            public void MyClick(int number)
            {
                if (number != 2)
                {
                    throw new ArgumentException();
                }

                IsClicked = true;
            }
            #endregion
        }
    }
}