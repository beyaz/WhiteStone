using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGeneration
{
    class WriterContext
    {
        #region Public Properties
        public List<string>        ClassBody       { get; set; }



        public string              ClassName       { get; set; }
        public List<string>        ConstructorBody { get; set; }
        public bool                HasWorkflow     { get; set; }
        public List<string>        Imports         { get; set; }
        public PaddedStringBuilder Output          { get; set; }

        public List<string> Page { get; set; }

        

        public RequestIntellisenseData RequestIntellisenseData { get; set; }
        public ScreenInfo              ScreenInfo              { get; set; }
        public SolutionInfo            SolutionInfo            { get; set; }
        #endregion
    }
}