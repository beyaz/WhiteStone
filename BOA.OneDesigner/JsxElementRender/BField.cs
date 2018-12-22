﻿using System;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.JsxElementRender
{
    [Serializable]
    public abstract class BField
    {
        public string Indent { get; set; }
        #region Public Properties
        public string BindingPath { get; set; }
        public string Label       { get; set; }

        public string SnapName
        {
            get
            {
                var typeName = GetType().Name.RemoveFromStart("B");

                return typeName[0].ToString().ToLowerTR() + typeName.Substring(1) + BindingPath;
            }
        }
        #endregion
    }
}