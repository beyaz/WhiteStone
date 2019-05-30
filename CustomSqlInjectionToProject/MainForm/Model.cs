using System;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using WhiteStone.UI.Container.Mvc;

namespace CustomSqlInjectionToProject.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public string CheckInComment        { get; set; }
        public bool   FinishTimer           { get; set; }
        public string ProfileId            { get; set; }
        public bool   StartTimer            { get; set; }

        public ProcessInfo CustomSqlGenerationOfProfileIdProcess { get; set; }
        #endregion
    }
}