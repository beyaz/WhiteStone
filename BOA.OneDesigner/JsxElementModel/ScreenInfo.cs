using System;
using System.Collections.Generic;
using BOA.OneDesigner.AppModel;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class ScreenInfo
    {
        #region Public Properties
        public string FormType                 { get; set; }
        public object JsxModel                 { get; set; }
        public string MessagingGroupName       { get; set; }
        public string OutputTypeScriptFileName { get; set; }
        public string RequestName              { get; set; }

        public List<Aut_ResourceAction> ResourceActions { get; set; }
        public string                   ResourceCode    { get; set; }
        public DateTime?                SystemDate      { get; set; }
        public string                   TfsFolderName   { get; set; }
        public string                   UserName        { get; set; }
        #endregion
    }
}