using System.Collections.Generic;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static int v = 1;
        #endregion

        #region Public Methods
        public void Generate()
        {
            Model.StartTimer = true;
        }

        public void GetCapture()
        {
            Model.ProcessIndicatorValue = v++;
            Model.ProcessIndicatorText = Model.ProcessIndicatorValue.ToString();
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ProcessIndicatorValue = 44,
                ProcessIndicatorText = "Ready",

                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Generate),
                        Text       = "Generate"
                    }
                }
            };
        }
        #endregion
    }
}