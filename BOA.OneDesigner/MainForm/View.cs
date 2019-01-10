using System.IO;
using System.Windows;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
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
            SM.Set(Host);

            Controller.Host = Host;

            UIBuilderHelper.RegisterElements();

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");

            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            _propertyEditorContainer.Host = Host;
            DesignSurface.Host            = Host;
            _toolBox.Host                 = Host;

            DesignSurface.AttachToEventBus();
            
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; } = new Host();
        #endregion

        #region Public Methods
        public static void Refresh(Host host, Model model, JsxElementDesignerSurface surface)
        {
            if (model == null)
            {
                return;
            }

            if (surface.DataContext == model)
            {
                surface.DataContext = null;
            }

            if (model.ScreenInfo?.JsxModel != null && surface.DataContext == model.ScreenInfo.JsxModel)
            {
                return;
            }

            if (model.ScreenInfo?.JsxModel != null)
            {
                surface.DataContext = model.ScreenInfo.JsxModel;
                host.EventBus.Publish(EventBus.RefreshFromDataContext);
            }

            else if (model.ScreenInfo != null && model.ScreenInfo.JsxModel == null && surface.DataContext != null)
            {
                model.ScreenInfo.JsxModel = surface.DataContext;
            }
            else
            {
                surface.DataContext = null;
                host.EventBus.Publish(EventBus.RefreshFromDataContext);
            }
        }

        public override void FireAction(string controllerPublicMethodName)
        {
            base.FireAction(controllerPublicMethodName);

            Refresh();
        }

        public void Refresh()
        {
            Refresh(Host, Model, DesignSurface);
        }
        #endregion
    }
}