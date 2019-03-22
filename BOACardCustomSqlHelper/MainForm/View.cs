using System.IO;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace BOACardCustomSqlHelper.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Constructors
        public View()
        {
            UIBuilderHelper.RegisterElements();

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");
        }
        #endregion
    }
}