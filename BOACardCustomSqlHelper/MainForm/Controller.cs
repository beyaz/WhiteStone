
using System.Collections.Generic;
using WhiteStone.UI.Container.Mvc;

namespace BOACardCustomSqlHelper.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        public void OnProfileIdChanged()
        {

        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ProfileIdCollection = new List<string> {"A", "B"}
            };
        }
    }
}