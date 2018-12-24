using System.Windows.Controls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BInput : Grid
    {
        #region Constructors
        public BInput()
        {
            this.LoadJson(@"
{
	rows:
	[
		{view:'TextBlock', Text:'{Binding " + nameof(JsxElementModel.BInput.Label) + @"}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Text:'{Binding " + nameof(JsxElementModel.BInput.BindingPath) + @"}' , IsReadOnly:true}        
	]
	
}");
        }
        #endregion

        #region Public Properties
        public JsxElementModel.BInput Data => (JsxElementModel.BInput) DataContext;
        #endregion
    }
}