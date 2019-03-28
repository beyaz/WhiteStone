using System.Collections.Generic;
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

        public List<ComponentGetValueInfo> FillRequestFromUI = new List<ComponentGetValueInfo>();

        public void GrabValuesToRequest(ComponentGetValueInfo data)
        {
            FillRequestFromUI.Add(data);
        }
    }

    public abstract class ComponentGetValueInfo
    {
        public string JsBindingPath { get; set; }
        public string SnapName      { get; set; }

        public abstract string GetCode();
    }

    public class ComponentGetValueInfoComboBox:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().value;";    
        }
    }

    public class ComponentGetValueInfoAccountComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().accountNumber;";    
        }
    }

    public class ComponentGetValueInfoAccountComponentSuffix:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().accountSuffix;";    
        }
    }
    public class ComponentGetValueInfoBranchComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().value;";    
        }
    }

    public class ComponentGetValueInfoCreditCardComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().clearCardNumber;";    
        }
    }

    public class ComponentGetValueInfoInput:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue();";    
        }
    }

    public class ComponentGetValueInfoParameterComponent:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getValue().value;";    
        }
    }

    public class ComponentGetValueInfoDataGridSelectedValueChangedBindingValue:ComponentGetValueInfo
    {
        public override string GetCode()
        {
            return $"{JsBindingPath} = this.snaps.{SnapName} && this.snaps.{SnapName}.getInstance().getSelectedItems()[0];";    
        }
    }
}