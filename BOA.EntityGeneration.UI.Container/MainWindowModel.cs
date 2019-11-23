using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.UI.Container
{
    [Serializable]
    public class MainWindowModel
    {
        #region Public Properties
        public string CheckinComment { get; set; }

        public  IReadOnlyList<string> SchemaNames { get; set; }
        #endregion
    }
}