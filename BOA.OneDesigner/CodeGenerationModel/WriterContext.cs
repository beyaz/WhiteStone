using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGenerationModel
{
    public class WriterContext
    {

        readonly List<TypeScriptMemberInfo> _classBody = new List<TypeScriptMemberInfo>();

        public WriterContext()
        {
            
        }
        #region Fields
        public List<ComponentGetValueInfo> FillRequestFromUI = new List<ComponentGetValueInfo>();
        #endregion

        #region Public Properties
        public Dictionary<string, bool> AllNames { get; set; } = new Dictionary<string, bool>();

        public List<string>               BeforeSetStateOnProxyDidResponse { get; set; }
        public bool                       CanWriteEvaluateActions          { get; set; }
        public IReadOnlyList<TypeScriptMemberInfo> ClassBody => _classBody;

        public string                   ClassName                                         { get; set; }
        public List<string>             ConstructorBody                                   { get; set; }
        public string                   DataContractAccessPathInWindowRequest             { get; set; }
        public bool                     DataContractAccessPathInWindowRequestIsCalculated { get; set; }
        public List<Aut_ResourceAction> EvaluatedActions                                  { get; set; }
        public bool                     HandleProxyDidRespondCallback                     { get; set; }
        public bool                     HasWorkflow                                       { get; set; }
        public List<string>             Imports                                           { get; set; }
        public bool                     IsBrowsePage                                      { get; set; }
        public PaddedStringBuilder      Output                                            { get; set; }

        public List<string> Page                                { get; set; }




        public RequestIntellisenseData RequestIntellisenseData { get; set; }
        public ScreenInfo              ScreenInfo              { get; set; }
        public SolutionInfo            SolutionInfo            { get; set; }

        public JsObject StateObjectWhenIncomingRequestIsSuccess { get; set; } = new JsObject();
        public bool     ThrowExceptionOnEmptyActionDefinition   { get; set; }
        #endregion

        #region Public Methods
        public void AddClassBody(TypeScriptMemberInfo info)
        {
            _classBody.Add(info);
            _classBody.Sort(TypeScriptMemberInfo.Compare);
        }

        public void AddClassBody(string code)
        {
            _classBody.Add(new TypeScriptMemberInfo {Code = code, IsMethod = true});
            _classBody.Sort(TypeScriptMemberInfo.Compare);
        }

        public void AddToBeforeSetStateOnProxyDidResponse(string line)
        {
            if (BeforeSetStateOnProxyDidResponse == null)
            {
                BeforeSetStateOnProxyDidResponse = new List<string>();
            }

            BeforeSetStateOnProxyDidResponse.Add(line);
        }

        public void GrabValuesToRequest(ComponentGetValueInfo data)
        {
            FillRequestFromUI.Add(data);
        }

        readonly List<JsBindingPathInfo> _usedBindingPathInRenderMethod = new List<JsBindingPathInfo>();

        public IReadOnlyList<JsBindingPathInfo> UsedBindingPathInRenderMethod => _usedBindingPathInRenderMethod;

        public void PushVariablesToRenderScope(JsBindingPathInfo data)
        {
            _usedBindingPathInRenderMethod.Add(data);
        }
        #endregion
    }
}