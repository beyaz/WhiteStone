using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.Common.Helpers;
using BOA.Jaml;

namespace WhiteStone.UI.Container
{
    public class Designer : Grid
    {
        #region Fields
        public Grid ContentGrid;
        #endregion

        #region Constructors
        public Designer()
        {
            const string ui = @"
{
    view:'Grid',
    cols:[
		{view:'TextArea', Gravity:2, Text:'SourceText', KeyUp:'TextArea_OnKeyUp' },
        {view:'GridSplitter'},
        {view:'Grid', Name:'ContentGrid', Gravity:4 }
	]
	
}
";
            var builder = new Builder
            {
                View        = this,
                DataContext = this
            };

            builder.SetJson(ui).Build();
        }
        #endregion

        #region Public Methods
        public static void Start()
        {
            Debug.Assert(Application.Current.MainWindow != null, "Application.Current.MainWindow != null");

            Application.Current.MainWindow.Content = new Designer();
        }

        public void TextArea_OnKeyUp(object sender, InputEventArgs args)
        {
            try
            {
                var builder = new Builder
                {
                    DataContext = this
                };

                var ui = builder.SetJson(SourceText).Build().View;

                ContentGrid.Children.Clear();

                ContentGrid.Children.Add(ui);
            }
            catch (Exception e)
            {
                App.ShowErrorNotification(e.ToString());
            }
        }
        #endregion

        #region SourceText
        public static readonly DependencyProperty SourceTextProperty = DependencyProperty.Register("SourceText", typeof(string), typeof(Designer), new PropertyMetadata(default(string)));

        public string SourceText
        {
            get { return (string) GetValue(SourceTextProperty); }
            set { SetValue(SourceTextProperty, value); }
        }
        #endregion
    }
}