using System;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {
        public int ProcessIndicatorValue { get; set; }
        public string ProcessIndicatorText { get; set; }
        public bool StartTimer { get; set; }
    }
}