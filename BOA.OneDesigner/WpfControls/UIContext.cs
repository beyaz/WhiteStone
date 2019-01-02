using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.PropertyEditors;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public static class UIBuilderHelper
    {
        #region Public Methods
        public static void RegisterElements()
        {
            Builder.RegisterElementCreation("Surface", typeof(JsxElementDesignerSurface));
            Builder.RegisterElementCreation("ToolBox", typeof(ToolBox));
            Builder.RegisterElementCreation("PropertyEditorContainer", typeof(PropertyEditorContainer));
            Builder.RegisterElementCreation("RequestIntellisenseTextBox", typeof(RequestIntellisenseTextBox));
            Builder.RegisterElementCreation("MessagingIntellisenseTextBox", typeof(MessagingIntellisenseTextBox));
            Builder.RegisterElementCreation("LabelEditor", typeof(LabelEditor));
            Builder.RegisterElementCreation("BDataGridEditor", typeof(BDataGridEditor));
            Builder.RegisterElementCreation("BInputEditor", typeof(BInputEditor));
        }
        #endregion
    }
}