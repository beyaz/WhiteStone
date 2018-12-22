using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{

    [Serializable]
    public class  Model:ModelBase
    {
        
    }

    public class Controller : ControllerBase<Model>
    {

    }
    public class View:WindowBase<Model,Controller>
    {

        public View()
        {

            

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");
        }

       
    }
}
