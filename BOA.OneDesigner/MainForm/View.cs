using System.IO;
using System.Windows;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Fields
        public PropertyEditorContainer   _propertyEditorContainer;
        public ToolBox                   _toolBox;
        public JsxElementDesignerSurface DesignSurface;
        #endregion

        #region Constructors
        public View()
        {
            UIContext.RegisterElements();

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");

            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            _propertyEditorContainer.Host = Host;
            DesignSurface.Host            = Host;
            _toolBox.Host                 = Host;
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; } = new Host();
        #endregion

        #region Public Methods
        public override void FireAction(string controllerPublicMethodName)
        {
            base.FireAction(controllerPublicMethodName);

            Refresh();
        }

        public void Refresh()
        {
            if (Model == null)
            {
                return;
            }

            if (Model.ScreenInfo.JsxModel != null && DesignSurface.DataContext == Model.ScreenInfo.JsxModel)
            {
                return;
            }

            if (Model?.ScreenInfo?.JsxModel != null)
            {
                DesignSurface.DataContext = Model.ScreenInfo.JsxModel;
                Host.EventBus.Publish(EventBus.RefreshFromDataContext);
            }

            else if (Model?.ScreenInfo != null && Model.ScreenInfo.JsxModel == null && DesignSurface.DataContext != null)
            {
                Model.ScreenInfo.JsxModel = DesignSurface.DataContext;
            }
            else
            {
                DesignSurface.DataContext = null;
                Host.EventBus.Publish(EventBus.RefreshFromDataContext);
            }
        }
        #endregion
    }
}