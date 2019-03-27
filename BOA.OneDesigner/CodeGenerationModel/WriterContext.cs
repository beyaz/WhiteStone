﻿using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGenerationModel
{
    public class WriterContext
    {
        #region Public Properties
        public Dictionary<string, bool> AllNames { get; set; } = new Dictionary<string, bool>();

        public List<string>               BeforeSetStateOnProxyDidResponse { get; set; }
        public bool                       CanWriteEvaluateActions          { get; set; }
        public List<TypeScriptMemberInfo> ClassBody                        { get; set; }

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
        public List<string> RenderMethodRequestRelatedVariables { get; set; } = new List<string>();

        public RequestIntellisenseData RequestIntellisenseData { get; set; }
        public ScreenInfo              ScreenInfo              { get; set; }
        public SolutionInfo            SolutionInfo            { get; set; }

        public JsObject StateObjectWhenIncomingRequestIsSuccess { get; set; } = new JsObject();
        public bool     ThrowExceptionOnEmptyActionDefinition   { get; set; }
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

        public void AddToBeforeSetStateOnProxyDidResponse(string line)
        {
            if (BeforeSetStateOnProxyDidResponse == null)
            {
                BeforeSetStateOnProxyDidResponse = new List<string>();
            }

            BeforeSetStateOnProxyDidResponse.Add(line);
        }
        #endregion

        public List<string> FillRequestFromUI = new List<string>();

        public void GrabValuesToRequest(string line)
        {
            FillRequestFromUI.Add(line);
        }
    }
}