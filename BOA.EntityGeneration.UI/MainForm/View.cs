using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.EntityGeneration.UI.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {

    }

    public class Controller : ControllerBase<Model>
    {

    }
    public class View : WindowBase<Model, Controller>
    {
    }
}
