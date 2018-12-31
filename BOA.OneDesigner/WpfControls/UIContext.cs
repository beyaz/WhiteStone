using System.Collections.Generic;
using System.Windows;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOAPlugins.Messaging;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public static class UIContext
    {

        public static IList<PropertyInfo> MessagingPropertyNames { get; set; }

        #region Public Properties
        public static UIElement DraggingElement { get; set; }

        public static Point DraggingElementStartPoint { get; set; }

        public static IReadOnlyList<string> RequestPropertyIntellisense { get; set; }
        public static IReadOnlyList<string> RequestStringPropertyIntellisense { get; set; }

        public static UIElement SelectedElement => DraggingElement;
        #endregion

        public static IDatabase Database;

        #region Public Methods
        public static void RegisterElements()
        {

            Database = new JsonFile();


            var tfsFolderNames = Database.GetTfsFolderNames();

            if (tfsFolderNames == null)
            {
                tfsFolderNames = TfsHelper.GetFolderNames();

                (Database as JsonFile)?.SaveTfsFolderNames(tfsFolderNames);
            }

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