using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.UIBuilding;
using FeserWard.Controls;

namespace WhiteStone.UI.Container
{
    public class IntellisenseTextBox : Grid
    {
        #region Constants
        const string UITemplate = @"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding Label}', MarginBottom:5, IsBold:true},
        {view:'Intellibox', SelectAllOnFocus:true,  
         SelectedValue:'{Binding Text}',
         Name:'IntellisenseBox',
         DataProvider:'{Binding QueryProvider,Mode=OneWay}'}
	]
	
}";
        #endregion

        #region Fields
        public Intellibox IntellisenseBox;
        #endregion

        #region Constructors
        public IntellisenseTextBox()
        {
            DataContext = this;

            this.LoadJson(UITemplate);
        }
        #endregion

        #region Public Methods
        public static UIElement On(Builder builder, Node node)
        {
            if (node.UI == nameof(Intellibox).ToUpperEN())
            {
                return new Intellibox();
            }

            if (node.UI == nameof(TextBox).ToUpperEN() &&
                node.HasProperty(nameof(IntellisenseSource)))
            {
                return new IntellisenseTextBox();
            }

            return null;
        }
        #endregion

        #region Label
        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string)));
        #endregion

        #region  Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region QueryProvider
        public static readonly DependencyProperty QueryProviderProperty = DependencyProperty.Register(
                                                                                                      "QueryProvider", typeof(IIntelliboxResultsProvider), typeof(IntellisenseTextBox), new PropertyMetadata(default(IIntelliboxResultsProvider)));

        public IIntelliboxResultsProvider QueryProvider
        {
            get { return (IIntelliboxResultsProvider) GetValue(QueryProviderProperty); }
            set { SetValue(QueryProviderProperty, value); }
        }
        #endregion

        #region IntellisenseSource
        public static readonly DependencyProperty IntellisenseSourceProperty = DependencyProperty.Register("IntellisenseSource", typeof(IReadOnlyCollection<string>), typeof(IntellisenseTextBox), new PropertyMetadata(default(IReadOnlyCollection<string>),OnIntellisenseSourceChanged));

        static void OnIntellisenseSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var intellisenseTextBox = d as IntellisenseTextBox;
            if (intellisenseTextBox == null)
            {
                return;
            }

            if (e.Property!= IntellisenseSourceProperty)
            {
                return;
            }

            intellisenseTextBox.IntellisenseBox.DataProvider = new StringCollectionProvider
            {
                Items = intellisenseTextBox.IntellisenseSource
            };

        }

        public IReadOnlyCollection<string> IntellisenseSource
        {
            get { return (IReadOnlyCollection<string>) GetValue(IntellisenseSourceProperty); }
            set { SetValue(IntellisenseSourceProperty, value); }
        }
        #endregion



        class StringCollectionProvider : IIntelliboxResultsProvider
        {
            public IReadOnlyCollection<string> Items { get; set; }

            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                return Items.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
            }
            #endregion

            #region Methods
            protected virtual IEnumerable<string> Sort(IEnumerable<string> preResults)
            {
                return preResults.OrderByDescending(s => s.Length);
            }

           
            #endregion
        }

    }
}