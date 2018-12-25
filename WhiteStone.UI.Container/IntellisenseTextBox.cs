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
        #region QueryProvider
        public static readonly DependencyProperty QueryProviderProperty = DependencyProperty.Register(
                                                                                                      "QueryProvider", typeof(IIntelliboxResultsProvider), typeof(IntellisenseTextBox), new PropertyMetadata(default(IIntelliboxResultsProvider)));

        public IIntelliboxResultsProvider QueryProvider
        {
            get { return (IIntelliboxResultsProvider) GetValue(QueryProviderProperty); }
            set { SetValue(QueryProviderProperty, value); }
        }

        #endregion



        public Intellibox IntellisenseBox;

        public static UIElement On(CustomUIMarkupLanguage.UIBuilding.Builder builder, Node node)
        {
            if (node.UI== nameof(Intellibox).ToUpperEN())
            {
                return new Intellibox();
            }

            if (node.UI == nameof(TextBox).ToUpperEN() && 
                node.HasProperty("IntellisenseSource") )
            {
                return new IntellisenseTextBox();
            }

            return null;
        }

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


        
        #region Static Fields
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(default(string)));
        #endregion

        #region Constructors
        public IntellisenseTextBox()
        {
            DataContext = this;

            this.LoadJson(UITemplate);
        }
        #endregion

        #region Public Properties
        public string Label
        {
            get { return (string) GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion
    }
}