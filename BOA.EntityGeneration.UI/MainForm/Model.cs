using System;
using System.Collections.Generic;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public ProcessContract AllSchemaGenerationProcess          { get; set; }
        public bool            AllSchemaGenerationProcessIsVisible { get; set; }
        public string          CheckInComment                      { get; set; }
        public bool            FinishTimer                         { get; set; }

        public ProcessContract             SchemaGenerationProcess { get; set; }
        public string                      SchemaName              { get; set; }
        public IReadOnlyCollection<string> SchemaNames             { get; set; }
        public bool                        StartTimer              { get; set; }
        #endregion
    }
}