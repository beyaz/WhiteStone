﻿using System;
using System.Collections.Generic;
using WhiteStone.UI.Container.Mvc;

namespace BOACardCustomSqlHelper.MainForm
{
    /// <summary>
    ///     The model
    /// </summary>
    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public IReadOnlyCollection<string> ProfileIdCollection { get; set; }
        #endregion
    }
}