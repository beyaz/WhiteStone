using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BInputWpf : Grid
    {
        #region Constructors
        public BInputWpf()
        {
            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;

            this.LoadJson(@"
{
    Margin:10,
	rows:
	[
		{view:'TextBlock', Text:'{Binding " + nameof(BInput.Label) + @"}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Text:'{Binding " + nameof(BInput.BindingPath) + @"}' , IsReadOnly:true}        
	]
	
}");
        }
        #endregion

        #region Public Properties
        public BInput Data => (BInput) DataContext;
        #endregion

        #region Methods
        void BInput_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        void BInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        #endregion
    }
}