using System;

namespace BOA.OneDesigner.AppModel
{
    [Serializable]
    public class Aut_Resource
    {
        public string Name         { get; set; }
        public string ResourceCode { get; set; }
        public override string ToString()
        {
            return ResourceCode;
        }
    }
}