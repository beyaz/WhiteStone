using BOAPlugins.Utility;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace BOAPlugins.VSIntegration.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Constructors
        public View()
        {
            CloseOnEscapeCharacter = true;

            this.LoadJsonFile(ConstConfiguration.PluginDirectory + "VSIntegration\\MainForm\\View.json");
        }
        #endregion

    }
}