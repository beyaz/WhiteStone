﻿using System;
using System.Collections.Generic;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class TabControl
    {
        #region Public Properties
        public IReadOnlyList<TabPage> TabPages { get; set; }
        #endregion
    }

    [Serializable]
    public class TabPage
    {
       

        #region Public Properties
        public IReadOnlyList<BCard> Cards { get; set; }
        public string               Title { get; set; }
        #endregion
    }
}