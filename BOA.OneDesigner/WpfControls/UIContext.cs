using System.Collections.Generic;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.PropertyEditors;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public static class UIContext
    {
        #region Static Fields
        public static IDatabase Database;
        #endregion

        #region Public Properties
        public static IReadOnlyList<string> RequestPropertyIntellisense       { get; set; }
        public static IReadOnlyList<string> RequestStringPropertyIntellisense { get; set; }
        #endregion

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