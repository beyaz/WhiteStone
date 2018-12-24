using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        public override void OnViewLoaded()
        {
           Model = new Model();
        }
    }
}