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
            Builder.RegisterElementCreation("SizeEditor", typeof(SizeEditor));
            Builder.RegisterElementCreation("BDataGridEditor", typeof(BDataGridEditor));
            
            Builder.RegisterElementCreation("WideEditor", typeof(WideEditor));
            Builder.RegisterElementCreation("HorizontalLocationEditor", typeof(HorizontalLocationEditor));
            // Builder.RegisterElementCreation("ResourceCodeTextBox", typeof(ResourceCodeTextBox));
            Builder.RegisterElementCreation("ResourceCodeTextBox", typeof(ResourceCodeTextBox));
            
            Builder.RegisterElementCreation("OrchestrationIntellisense", typeof(OrchestrationIntellisense));
            
            
            
        }
        #endregion
    }
}