using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.UIBuilding;
using FeserWard.Controls;
using ReflectionHelper = CustomUIMarkupLanguage.ReflectionHelper;

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

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Builder.RegisterElementCreation(On);
            }

            this.LoadJson(UITemplate);

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Builder.RegisterElementCreation(On);
                return;
            }

            IntellisenseBox.MaxResults = 5;

            EdiTextBox.KeyUp += (s, e) => { UpdateTextFromEditBox(); };

            IntellisenseBox.SearchCompleted += UpdateTextFromEditBox;

            IntellisenseBox.SingleClickToSelectResult = true;
            IntellisenseBox.MinimumPrefixLength       = 0;

            // https://github.com/joefeser/intellibox
        }
        #endregion

        #region Public Events
        public event Action TextChanged;
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

        #region Methods
        void OnTextChanged()
        {
            TextChanged?.Invoke();
        }

        void UpdateTextFromEditBox()
        {
            Text = EdiTextBox.Text;
        }
        #endregion

        class StringCollectionProvider : IIntelliboxResultsProvider
        {
            #region Public Properties
            public IReadOnlyCollection<string> Items { get; set; }
            #endregion

            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                return Items.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
            }
            #endregion

            #region Methods
            protected IEnumerable<string> Sort(IEnumerable<string> preResults)
            {
                return preResults.OrderByDescending(s => s.Length);
            }
            #endregion
        }

        #region Label
        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string)));
        #endregion

        #region  Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string), OnTextChanged));

        static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var intellisenseTextBox = d as IntellisenseTextBox;
            if (intellisenseTextBox == null)
            {
                return;
            }

            intellisenseTextBox.IntellisenseBox.SelectedValue = (string) e.NewValue;
            intellisenseTextBox.IntellisenseBox.WatermarkText = (string) e.NewValue;

            intellisenseTextBox.OnTextChanged();
        }

        TextBox EdiTextBox => (TextBox) ReflectionHelper.GetField(typeof(Intellibox), "PART_EDITFIELD", false, false, true, true).GetValue(IntellisenseBox);

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
        public static readonly DependencyProperty IntellisenseSourceProperty = DependencyProperty.Register("IntellisenseSource", typeof(IReadOnlyCollection<string>), typeof(IntellisenseTextBox), new PropertyMetadata(default(IReadOnlyCollection<string>), OnIntellisenseSourceChanged));

        static void OnIntellisenseSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var intellisenseTextBox = d as IntellisenseTextBox;
            if (intellisenseTextBox == null)
            {
                return;
            }

            if (e.Property != IntellisenseSourceProperty)
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
    }
}