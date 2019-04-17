using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class ResourceActionEditor : GroupBox
    {
        #region Fields
        public ActionInfoEditor _onClickActionInfoEditor;

        public ResourceCodeTextBox ResourceCodeTextBox;
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        Aut_ResourceAction Model => (Aut_ResourceAction) DataContext;
        #endregion

        #region Public Methods
        public void LoadUI()
        {
            if (Model.OnClickAction == null)
            {
                Model.OnClickAction = new ActionInfo();
            }

            this.LoadJson(@"

{
     Header:'Resource Action',
     Margin:5,
     Content:
     {
        ui:'StackPanel',
	    rows:
        [
		    {
                ui                          : 'RequestIntellisenseTextBox', 
                ShowOnlyBooleanProperties   : true,    
                Margin                      : 5, 
                Text                        : '{Binding " + nameof(Model.IsVisibleBindingPath) + @"}',     
                Label                       : 'Is Visible Binding Path'
            }
            ,
            {
                ui                          : 'RequestIntellisenseTextBox',
                Margin                      : 5,
                Text                        : '{Binding " + nameof(Model.IsEnableBindingPath) + @"}',
                Label                       : 'Is Enable Binding Path',
                ShowOnlyBooleanProperties   : true
            }
            ,
            {
                ui          : 'ActionInfoEditor',
                Header      : 'On Click',
                Name        : '_onClickActionInfoEditor'
            }           

	    ]
     }

}


");

            _onClickActionInfoEditor.Load(Host, Model.OnClickAction);
        }
        #endregion
    }
}