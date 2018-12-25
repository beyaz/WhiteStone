using System;
using System.Collections.Generic;
using System.Windows;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public static class UIContext
    {
        public static void RegisterElements()
        {
            Builder.RegisterElementCreation("Surface",typeof(JsxElementDesignerSurface));
            Builder.RegisterElementCreation("ToolBox",typeof(ToolBox));
            Builder.RegisterElementCreation("PropertyEditorContainer",typeof(PropertyEditorContainer));
            Builder.RegisterElementCreation("RequestIntellisenseTextBox",typeof(RequestIntellisenseTextBox));
        }


        #region Static Fields
        static readonly List<JsxElementDesignerSurface> DesignerSurfaces = new List<JsxElementDesignerSurface>();
        #endregion

        #region Public Events
        public static event Action DragElementSelected;
        #endregion

        #region Public Properties
        public static UIElement DraggingElement { get; set; }


        public static UIElement SelectedElement => DraggingElement;

        public static Point DraggingElementStartPoint { get; set; }
        #endregion

        #region Public Methods
        public static void OnDragElementSelected()
        {
            DragElementSelected?.Invoke();
        }

        public static void Register(JsxElementDesignerSurface designerSurface)
        {
            DesignerSurfaces.Add(designerSurface);
        }
        #endregion
    }
}