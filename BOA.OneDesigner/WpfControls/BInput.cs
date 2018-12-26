using System.Windows.Controls;
using System.Windows.Input;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{

   
    public class BInputWpf : Grid,IJsxElementDesignerSurfaceItem
    {
        public JsxElementDesignerSurface Surface { get; set; }

        public IDropLocationContainer Container { get; set; }
        

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
		{view:'TextBlock', Text:'{Binding " + nameof(JsxElementModel.BInput.Label) + @"}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Text:'{Binding " + nameof(JsxElementModel.BInput.BindingPath) + @"}' , IsReadOnly:true}        
	]
	
}");
        }

        private void BInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        void BInput_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }
        #endregion

        #region Public Properties
        public JsxElementModel.BInput Data => (JsxElementModel.BInput) DataContext;
        #endregion
    }
}