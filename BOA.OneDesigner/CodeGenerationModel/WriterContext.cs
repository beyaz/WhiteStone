using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGeneration;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGenerationModel
{
    public class TypeScriptMemberInfo
    {

        

        #region Public Properties
        public string Code          { get; set; }
        public bool   IsConstructor { get; set; }
        public bool   IsField       { get; set; }
        public bool   IsMethod      { get; set; }
        public bool   IsRender      { get; set; }
        #endregion

        #region Public Methods
        public static int Compare(TypeScriptMemberInfo left, TypeScriptMemberInfo right)
        {
            if (left.IsRender && right.IsRender)
            {
                return 0;
            }

            if (left.IsConstructor && right.IsConstructor)
            {
                return 0;
            }

            if (left.IsField && right.IsField)
            {
                return 0;
            }

            if (left.IsMethod && right.IsMethod)
            {
                return 0;
            }

            

            if (left.IsField)
            {
                return -1;
            }

            if ( right.IsField)
            {
                return 1;
            }

            if (left.IsConstructor)
            {
                return -1;
            }

            if (right.IsConstructor)
            {
                return 1;
            }

            if (left.IsMethod)
            {
                return -1;
            }

            return 1;
        }
        #endregion
    }

    public class WriterContext
    {
        public WriterContext()
        {
            
        }
        public JsObject StateObjectWhenIncomingRequestIsSuccess { get; set; } = new JsObject();

        public List<string> BeforeSetStateOnProxyDidResponse { get; set; }

        public void AddToBeforeSetStateOnProxyDidResponse(string line)
        {
            if (BeforeSetStateOnProxyDidResponse == null)
            {
                BeforeSetStateOnProxyDidResponse = new List<string>();
            }

            BeforeSetStateOnProxyDidResponse.Add(line);
        }


        public Dictionary<string,bool> AllNames { get; set; } = new Dictionary<string, bool>();

        #region Public Properties
        public List<TypeScriptMemberInfo> ClassBody { get; set; }

        public string              ClassName       { get; set; }
        public List<string>        ConstructorBody { get; set; }
        public bool                HasWorkflow     { get; set; }
        public List<string>        Imports         { get; set; }
        public PaddedStringBuilder Output          { get; set; }

        public List<string> Page { get; set; }

        public RequestIntellisenseData RequestIntellisenseData { get; set; }
        public ScreenInfo              ScreenInfo              { get; set; }
        public SolutionInfo            SolutionInfo            { get; set; }
        public List<Aut_ResourceAction> EvaluatedActions { get; set; }
        public bool CanWriteEvaluateActions { get; set; }
        public string DataContractAccessPathInWindowRequest { get; set; }
        public bool DataContractAccessPathInWindowRequestIsCalculated { get; set; }
        public bool IsBrowsePage { get; set; }
        public List<string> RenderMethodRequestRelatedVariables { get; set; } = new List<string>();
        public bool ThrowExceptionOnEmptyActionDefinition { get; set; }
        public bool HandleProxyDidRespondCallback { get; set; }
        #endregion

        #region Public Methods
        public void AddClassBody(TypeScriptMemberInfo info)
        {
            ClassBody.Add(info);
        }

        public void AddClassBody(string code)
        {
            ClassBody.Add(new TypeScriptMemberInfo {Code = code, IsMethod = true});
        }
        #endregion
    }
}