using System;

namespace BOA.OneDesigner.JsxElementModel
{
    [Serializable]
    public class ScreenInfo
    {
        public string TfsFolderName { get; set; }
        public string RequestName   { get; set; }
        public string FormType      { get; set; }
        public string MessagingGroupName { get; set; }
        public object JsxModel      { get; set; }
        public string ResourceCode { get; set; }
    }
}