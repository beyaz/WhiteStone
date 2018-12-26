using System.Collections.Generic;
using System.Windows;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.MainForm;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public static class UIContext
    {
        public static void RegisterElements()
        {
            InitialConfigCache.TryLoadFromCache();
            if (!SM.HasValue<InitialConfig>())
            {
                SM.Set(new InitialConfig
                {
                    TfsFolderNames = TfsHelper.GetFolderNames(),
                    ScreenInfoList = new List<ScreenInfo>()
                });

                InitialConfigCache.Save();    
                
            }
            
                

            Builder.RegisterElementCreation("Surface",typeof(JsxElementDesignerSurface));
            Builder.RegisterElementCreation("ToolBox",typeof(ToolBox));
            Builder.RegisterElementCreation("PropertyEditorContainer",typeof(PropertyEditorContainer));
            Builder.RegisterElementCreation("RequestIntellisenseTextBox",typeof(RequestIntellisenseTextBox));
        }


     

       

        #region Public Properties
        public static UIElement DraggingElement { get; set; }


        public static UIElement SelectedElement => DraggingElement;

        public static Point DraggingElementStartPoint { get; set; }
        #endregion
        
    }
}