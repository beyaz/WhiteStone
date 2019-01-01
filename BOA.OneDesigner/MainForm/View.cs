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
        public JsxElementDesignerSurface DesignSurface;
        #endregion

        #region Constructors
        public View()
        {
            UIContext.RegisterElements();

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");

            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            DesignSurface.Host = new Host();
        }
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
                EventBus2.Publish(EventBus2.RefreshFromDataContext);
            }

            else if (Model?.ScreenInfo != null && Model.ScreenInfo.JsxModel == null && DesignSurface.DataContext != null)
            {
                Model.ScreenInfo.JsxModel = DesignSurface.DataContext;
            }
            else
            {
                DesignSurface.DataContext = null;
                EventBus2.Publish(EventBus2.RefreshFromDataContext);
            }
        }
        #endregion
    }
}