namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public abstract class ComponentGetValueInfo
    {
        public string JsBindingPath { get; set; }
        public string SnapName      { get; set; }

        public abstract string GetCode();

        

        public const string Map = "map";
    }
}