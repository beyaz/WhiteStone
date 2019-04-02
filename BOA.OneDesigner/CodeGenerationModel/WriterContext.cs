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
        public WriterContext()
        {
            
        }
        #region Fields
        public readonly List<string> ConstructorBody = new List<string>();

        public readonly List<string> Imports = new List<string>();

        public readonly List<string> Page = new List<string>();

        readonly List<TypeScriptMemberInfo>  _classBody         = new List<TypeScriptMemberInfo>();
        readonly List<ComponentGetValueInfo> _fillRequestFromUI = new List<ComponentGetValueInfo>();

        readonly List<JsBindingPathInfo>  _usedBindingPathInRenderMethod = new List<JsBindingPathInfo>();
        readonly Dictionary<string, bool> _usedNames                     = new Dictionary<string, bool>();
        #endregion

        #region Constructors
        #endregion

        #region Public Properties
        public List<string>                        BeforeSetStateOnProxyDidResponse { get; set; }
        public bool                                CanWriteEvaluateActions          { get; set; }
        public IReadOnlyList<TypeScriptMemberInfo> ClassBody                        => _classBody;

        public string                               ClassName                                         { get; set; }
        public string                               DataContractAccessPathInWindowRequest             { get; set; }
        public bool                                 DataContractAccessPathInWindowRequestIsCalculated { get; set; }
        public List<Aut_ResourceAction>             EvaluatedActions                                  { get; set; }
        public IReadOnlyList<ComponentGetValueInfo> FillRequestFromUI                                 => _fillRequestFromUI;
        public bool                                 HandleProxyDidRespondCallback                     { get; set; }
        public bool                                 HasWorkflow                                       { get; set; }

        public bool                IsBrowsePage { get; set; }
        public PaddedStringBuilder Output       { get; set; }

        public RequestIntellisenseData RequestIntellisenseData { get; set; }
        public ScreenInfo              ScreenInfo              { get; set; }
        public SolutionInfo            SolutionInfo            { get; set; }

        public JsObject StateObjectWhenIncomingRequestIsSuccess { get; set; } = new JsObject();
        public bool     ThrowExceptionOnEmptyActionDefinition   { get; set; }

        public IReadOnlyList<JsBindingPathInfo> UsedBindingPathInRenderMethod => _usedBindingPathInRenderMethod;
        public bool HasTabControl { get; set; }
        public bool IsTabPage { get; set; }
        public string ExecuteWindowRequestFunctionAccessPath { get; set; }
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

        public bool ContainsUsedName(string name)
        {
            return _usedNames.ContainsKey(name);
        }

        public void GrabValuesToRequest(ComponentGetValueInfo data)
        {
            _fillRequestFromUI.Add(data);
        }

        public void PushUsedName(string name)
        {
            _usedNames[name] = true;
        }

        public void PushVariablesToRenderScope(JsBindingPathInfo data)
        {
            _usedBindingPathInRenderMethod.Add(data);
        }
        #endregion
    }
}