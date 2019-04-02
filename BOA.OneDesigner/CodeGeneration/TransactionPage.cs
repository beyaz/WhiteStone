using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class TransactionPage
    {
        #region Public Methods
        public static string Generate(ScreenInfo screenInfo)
        {
            var jsxModel = (DivAsCardContainer) screenInfo.JsxModel;

            var hasWorkflow  = screenInfo.FormType == FormType.TransactionPageWithWorkflow;
            var isBrowseForm = screenInfo.FormType == FormType.BrowsePage;
            var className    = screenInfo.RequestName.SplitAndClear(".").Last().RemoveFromEnd("Request");

            var writerContext = new WriterContext
            {
                ClassName    = className,
                HasWorkflow  = hasWorkflow,
                ScreenInfo   = screenInfo,
                IsBrowsePage = isBrowseForm,
                SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName),
                ThrowExceptionOnEmptyActionDefinition = false
            };

            writerContext.Imports.AddRange(new List<string>
            {
                "// One Designer Generated Code. Please do not edit manual.",
                "import * as React from \"react\"",
                "import { BFormManager } from \"b-form-manager\"",
                "import { getMessage } from \"b-framework\"",
                "import { ComponentSize } from \"b-component\""
            });

            if (isBrowseForm)
            {
                writerContext.Imports.Add("import { BrowsePage, BrowsePageComposer } from \"b-framework\"");
            }
            else
            {
                writerContext.Imports.Add("import { TransactionPage, TransactionPageComposer } from \"b-framework\"");
            }

            writerContext.RequestIntellisenseData = CecilHelper.GetRequestIntellisenseData(writerContext.SolutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName);
            if (writerContext.RequestIntellisenseData == null)
            {
                throw Error.RequestNotFound(screenInfo.RequestName);
            }

            WriteClass(writerContext, jsxModel);

            if (isBrowseForm)
            {
                writerContext.Page.Add($"export default BrowsePageComposer({className});");
            }
            else
            {
                writerContext.Page.Add($"export default TransactionPageComposer({className});");
            }

            writerContext.Page.Insert(0, string.Join(Environment.NewLine, writerContext.Imports.Distinct().ToList()));

            writerContext.Page.Insert(1, $"const WindowRequestFullName = \"{writerContext.ScreenInfo.RequestName}\";");

            var sb = new PaddedStringBuilder();
            foreach (var item in writerContext.Page)
            {
                sb.AppendLine();
                sb.AppendAll(item);
                sb.AppendLine();
            }

            return sb.ToString().Trim();
        }
        #endregion

        #region Methods
        static void CalculateDataField(WriterContext writerContext)
        {
            if (writerContext.RequestIntellisenseData.RequestPropertyIntellisense.Contains("Data"))
            {
                writerContext.DataContractAccessPathInWindowRequest             = "data";
                writerContext.DataContractAccessPathInWindowRequestIsCalculated = true;
            }
            else if (writerContext.RequestIntellisenseData.RequestPropertyIntellisense.Contains("DataContract"))
            {
                writerContext.DataContractAccessPathInWindowRequest             = "dataContract";
                writerContext.DataContractAccessPathInWindowRequestIsCalculated = true;
            }
            else
            {
                writerContext.DataContractAccessPathInWindowRequest = "?";
            }
        }

        static void CalculateEvaluatedActionStates(WriterContext writerContext)
        {
            var resourceActions = writerContext.ScreenInfo.ResourceActions;
            if (resourceActions == null)
            {
                writerContext.CanWriteEvaluateActions = false;
                return;
            }

            writerContext.EvaluatedActions = resourceActions.Where(x => x.IsVisibleBindingPath.HasValue() || x.IsEnableBindingPath.HasValue()).ToList();
            if (writerContext.EvaluatedActions.Count == 0)
            {
                writerContext.CanWriteEvaluateActions = false;
                return;
            }

            writerContext.CanWriteEvaluateActions = true;
        }

        static void OnWindowRequestFilled(WriterContext writerContext)
        {
            if (!writerContext.HasWorkflow)
            {
                return;
            }

            var sb = new PaddedStringBuilder();
            sb.AppendLine("onWindowRequestFilled(request: any)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const clonedWindowRequest: any = Object.assign({}, request.body);");
            sb.AppendLine();
            sb.AppendLine("clonedWindowRequest.hasWorkflow = false;");
            sb.AppendLine();
            sb.AppendLine("this.sendRequestToServer(clonedWindowRequest, \"LoadData\");");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());


        }
        static void ComponentWillMount(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Evaluates initial states of form.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("componentWillMount()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super.componentWillMount();");

            sb.AppendLine();
            sb.AppendLine("// on tab changed no need to go to orchestration");
            sb.AppendLine("if (this.state.windowRequest)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("const formData = this.state.pageParams.data;");
                sb.AppendLine();
                sb.AppendLine("const hasWorkflow = formData && formData.instanceId > 0;");
                sb.AppendLine("if (hasWorkflow)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine("return;");
                sb.PaddingCount--;
                sb.AppendLine("}");

                sb.AppendLine();

                sb.AppendLine("const firstRequest: any = {};");
                sb.AppendLine();
                sb.AppendLine("// is opening from another form with data parameter");
                sb.AppendLine("if (formData)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"firstRequest.{writerContext.DataContractAccessPathInWindowRequest} = formData;");
                sb.PaddingCount--;
                sb.AppendLine("}");

                sb.AppendLine();
                sb.AppendLine("this.sendRequestToServer(firstRequest, \"LoadData\");");
                
            }
            else
            {
                sb.AppendLine();
                if (writerContext.DataContractAccessPathInWindowRequestIsCalculated)
                {
                    sb.AppendLine("this.sendRequestToServer({ " + writerContext.DataContractAccessPathInWindowRequest + ": this.state.pageParams.data}, \"LoadData\");");
                }
                else
                {
                    sb.AppendLine("this.sendRequestToServer({}, \"LoadData\");");
                }
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void EvaluateActions(WriterContext writerContext)
        {
            if (!writerContext.CanWriteEvaluateActions)
            {
                return;
            }

            var sb = new PaddedStringBuilder();
            sb.AppendLine("evaluateActions()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const request = this.state.windowRequest;");

            foreach (var resourceAction in writerContext.EvaluatedActions)
            {
                if (resourceAction.IsVisibleBindingPath.HasValue())
                {
                    sb.AppendLine();

                    var bindingPath = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + resourceAction.IsVisibleBindingPath);

                    sb.AppendLine($"if ({bindingPath})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"this.visibleAction(\"{resourceAction.CommandName}\");");
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                    sb.AppendLine("else");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"this.hideAction(\"{resourceAction.CommandName}\");");
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }

                if (resourceAction.IsEnableBindingPath.HasValue())
                {
                    sb.AppendLine();

                    var bindingPath = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + resourceAction.IsEnableBindingPath);

                    sb.AppendLine($"if ({bindingPath})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"this.enableAction(\"{resourceAction.CommandName}\");");
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                    sb.AppendLine("else");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"this.disableAction(\"{resourceAction.CommandName}\");");
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void ExecuteWindowRequest(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Sends given requests to server.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("executeWindowRequest(orchestrationMethodName: string)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const clonedWindowRequest: any = Object.assign({}, this.state.windowRequest || {});");

            sb.AppendLine();
            sb.AppendLine("this.fillRequestFromUI(clonedWindowRequest);");

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("const hasWorkflow = clonedWindowRequest.workFlowInternalData && clonedWindowRequest.workFlowInternalData.instanceId > 0;");
                sb.AppendLine("if (hasWorkflow)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine("clonedWindowRequest.hasWorkflow = false;");
                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("this.sendRequestToServer(clonedWindowRequest, orchestrationMethodName);");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void FillWindowRequest(WriterContext writerContext)
        {
            var function = new FillWindowRequestFunctionDefinition{FillRequestFromUI = writerContext.FillRequestFromUI,HasTabControl = writerContext.HasTabControl}.GetCode();

            writerContext.AddClassBody(function);
        }

        static void ProxyDidRespond(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();
            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Proxies the did respond.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("proxyDidRespond(proxyResponse: GenericProxyResponse<any>)");
            sb.AppendLine("{");
            sb.PaddingCount++;


            sb.AppendLine("if (proxyResponse.requestClass !== WindowRequestFullName)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("return super.proxyDidRespond(proxyResponse);");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const { success, results, value } = proxyResponse.response;");

            sb.AppendLine();
            sb.AppendLine("if (!success)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (results == null)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("this.showStatusMessage(\"DeveloperError: Check incoming data from Developer Tools -> Network.\");");
            sb.AppendLine("return false;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const businessResult = results.find(r => r.severity === BOA.Common.Types.Severity.BusinessError);");
            sb.AppendLine("if (businessResult)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("this.showStatusMessage(businessResult.errorMessage);");
            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("BFormManager.showStatusErrorMessage(`Beklenmedik bir hata oluştu.${JSON.stringify(results, null, 2)}`, null);");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("if (value == null)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (success)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("BFormManager.showStatusErrorMessage(`Orch method:${proxyResponse.key} should return GenericResponse<${WindowRequestFullName}>`, null);");

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine("return false;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("const hasWorkflow = value.workFlowInternalData && value.workFlowInternalData.instanceId > 0;");
                sb.AppendLine("if (hasWorkflow)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine("const windowRequestInForm = this.getWindowRequest().body || {};");
                sb.AppendLine();
                sb.AppendLine("value.hasWorkflow = windowRequestInForm.hasWorkflow;");
                sb.AppendLine();
                sb.AppendLine("if (value.methodName === proxyResponse.key)");
                sb.AppendLine("{");
                sb.AppendLine("    value.methodName = windowRequestInForm.methodName;");
                sb.AppendLine("}");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("if (value.statusMessage)");
            sb.AppendLine("{");
            sb.AppendLine("    BFormManager.showStatusMessage(value.statusMessage);");
            sb.AppendLine("    value.statusMessage = null;");
            sb.AppendLine("}");

            

            
                sb.AppendLine();
                sb.AppendLine("this.setWindowRequest(");
                sb.AppendLine("{");
                sb.AppendLine("    body: value,");
                sb.AppendLine("    type: WindowRequestFullName");
                sb.AppendLine("});");
            

            if (writerContext.BeforeSetStateOnProxyDidResponse?.Count > 0)
            {
                sb.AppendLine();
                foreach (var line in writerContext.BeforeSetStateOnProxyDidResponse)
                {
                    sb.AppendAll(line);
                    sb.AppendLine();
                }
            }

            sb.AppendLine();
            sb.AppendWithPadding("this.setState(");

            writerContext.StateObjectWhenIncomingRequestIsSuccess.Add("windowRequest", "value");

            JsObjectInfoMultiLineWriter.Write(sb, writerContext.StateObjectWhenIncomingRequestIsSuccess);
            sb.Append(");");
            sb.Append(Environment.NewLine);



            if (writerContext.HasTabControl)
            {
                sb.AppendLine();
                sb.AppendLine("this.onProxyDidRespond.forEach(tabPage => tabPage.proxyDidRespond());");
            }

            if (writerContext.HandleProxyDidRespondCallback)
            {
                sb.AppendLine();
                sb.AppendLine("const me: any = this;");
                sb.AppendLine("if(me.internalProxyDidRespondCallback)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("me.internalProxyDidRespondCallback();");
                sb.AppendLine("me.internalProxyDidRespondCallback = null;");

                sb.PaddingCount--;
                sb.AppendLine("}");

            }

            
            if (writerContext.RequestIntellisenseData.HasPropertyLikeDialogResponse)
            {
                writerContext.Imports.Add("import { BDialogHelper } from \"b-dialog-box\";");

                sb.AppendLine("if(value.dialogResponse > 0)");
                sb.AppendLine("{");
                sb.AppendLine("    BDialogHelper.close(this, value.dialogResponse, null);");
                sb.AppendLine("}");
            }


            if (writerContext.CanWriteEvaluateActions)
            {
                sb.AppendLine();
                sb.AppendLine("this.evaluateActions();");
            }

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("if (success && this.executeWorkFlow)");
                sb.AppendLine("{");
                sb.AppendLine("    this.executeWorkFlow();");
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("return success;");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        /// <summary>
        ///     Renders the specified writer context.
        /// </summary>
        static void Render(WriterContext writerContext, DivAsCardContainer data)
        {
            var function = new RenderFunctionDefinition{WriterContext = writerContext,Data = data, WindowRequestAccessPath = "this.getWindowRequest().body"};

            writerContext.AddClassBody(new TypeScriptMemberInfo {Code = function.GetCode(), IsRender = true});
        }

        static void SendWindowRequestToServer(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Sends given requests to server.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("sendRequestToServer(request: any, orchestrationMethodName: string)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("request.methodName = orchestrationMethodName;");

            sb.AppendLine("const proxyRequest:any =");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("requestClass: WindowRequestFullName,");
            sb.AppendLine("key: orchestrationMethodName,");
            sb.AppendLine("requestBody: request,");
            sb.AppendLine("showProgress: false");

            sb.PaddingCount--;
            sb.AppendLine("}");

            if (writerContext.IsBrowsePage)
            {
                sb.AppendLine("this.proxyExecute(proxyRequest);");
            }
            else
            {
                sb.AppendLine("this.proxyTransactionExecute(proxyRequest);");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void WriteClass(WriterContext writerContext, DivAsCardContainer jsxModel)
        {
            var sb = new PaddedStringBuilder();

            CalculateEvaluatedActionStates(writerContext);
            CalculateDataField(writerContext);

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  The " + string.Join(" ", FileNamingHelper.GetWords(writerContext.ClassName)));
                sb.AppendLine("  */");
            }

            if (writerContext.IsBrowsePage)
            {
                sb.AppendLine($"class {writerContext.ClassName} extends BrowsePage");
            }
            else
            {
                sb.AppendLine($"class {writerContext.ClassName} extends TransactionPage");
            }

            sb.Append("{");
            sb.PaddingCount++;

            WriteWorkflowFields(writerContext);
           

            WriteOnActionClick(writerContext);
            OnWindowRequestFilled(writerContext);
            ComponentWillMount(writerContext);

            EvaluateActions(writerContext);
            SendWindowRequestToServer(writerContext);
            ExecuteWindowRequest(writerContext);
            Render(writerContext, jsxModel);
            ProxyDidRespond(writerContext);
            FillWindowRequest(writerContext);

            WriteConstructor(writerContext);

            WriteTabManagementFields(writerContext);


            foreach (var member in writerContext.ClassBody)
            {
                sb.AppendLine();
                sb.AppendAll(member.Code);
                sb.AppendLine();
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.Page.Add(sb.ToString());
        }

        static void WriteConstructor(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Creates a new instance of this class.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("constructor(props: BFramework.BasePageProps)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super(props);");
            sb.AppendLine();
            sb.AppendLine("this.connect(this);");
            sb.AppendLine();
            foreach (var line in writerContext.ConstructorBody)
            {
                sb.AppendLine(line);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(new TypeScriptMemberInfo {Code = sb.ToString(), IsConstructor = true});
        }

        static void WriteOnActionClick(WriterContext writerContext)
        {
            if (writerContext.ScreenInfo.ResourceActions == null)
            {
                return;
            }

            var resourceActions = writerContext.ScreenInfo.ResourceActions;

            var sb = new PaddedStringBuilder();
            if (writerContext.HasWorkflow)
            {
                #region onActionClick
                if (RenderHelper.IsCommentEnabled)
                {
                    sb.AppendLine("/**");
                    sb.AppendLine("  *  Handle click actions of page commands.");
                    sb.AppendLine("  */");
                }

                sb.AppendLine("onActionClick(command: BOA.Common.Types.ResourceActionContract, executeWorkFlow: () => void)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("this.executeWorkFlow = executeWorkFlow;");
                sb.AppendLine();

                foreach (var resourceAction in resourceActions)
                {
                    sb.AppendLine($"if (command.commandName === \"{resourceAction.CommandName}\")");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    RenderHelper.WriteButtonAction(sb, new ButtonActionInfoFunction
                    {
                        OrchestrationMethodName                          = resourceAction.OrchestrationMethodName,
                        OpenFormWithResourceCode                         = resourceAction.OpenFormWithResourceCode,
                        OpenFormWithResourceCodeDataParameterBindingPath = resourceAction.OpenFormWithResourceCodeDataParameterBindingPath,
                        DesignerLocation                                 = resourceAction.Name,
                        WriterContext = writerContext
                    });

                    sb.AppendLine("return false;");
                    sb.PaddingCount--;
                    sb.AppendLine("}");

                    sb.AppendLine();
                }

                sb.AppendLine("return true;");

                sb.PaddingCount--;
                sb.AppendLine("}");
                #endregion
            }
            else
            {
                #region onActionClick
                if (RenderHelper.IsCommentEnabled)
                {
                    sb.AppendLine("/**");
                    sb.AppendLine("  *  Handle click actions of page commands.");
                    sb.AppendLine("  */");
                }

                sb.AppendLine("onActionClick(command: BOA.Common.Types.ResourceActionContract)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                foreach (var resourceAction in resourceActions)
                {
                    sb.AppendLine($"if (command.commandName === \"{resourceAction.CommandName}\")");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    #region action body
                    if (resourceAction.OpenFormWithResourceCode.HasValue() && resourceAction.OrchestrationMethodName.HasValue())
                    {
                        throw Error.InvalidOperation("'Open Form With Resource Code' ve 'Orchestration Method Name' aynı anda dolu olamaz." + resourceAction.CommandName);
                    }

                    if (writerContext.ThrowExceptionOnEmptyActionDefinition)
                    {
                        if (resourceAction.OpenFormWithResourceCode.IsNullOrWhiteSpace() && resourceAction.OrchestrationMethodName.IsNullOrWhiteSpace())
                        {
                            var message = "'Open Form With Resource Code' veya 'Orchestration Method Name' dan biri dolu olmalıdır." + resourceAction.CommandName;
                            throw Error.InvalidOperation(message);
                        }
                    }
                   

                    if (resourceAction.OpenFormWithResourceCode.HasValue())
                    {
                        if (resourceAction.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
                        {
                            var bindingPathForDataParameter = "this.state.windowRequest." + TypescriptNaming.NormalizeBindingPath(resourceAction.OpenFormWithResourceCodeDataParameterBindingPath);

                            sb.AppendLine($"BFormManager.show(\"{resourceAction.OpenFormWithResourceCode.Trim()}\", /*data*/{bindingPathForDataParameter}, true,null);");
                        }
                        else
                        {
                            sb.AppendLine($"BFormManager.show(\"{resourceAction.OpenFormWithResourceCode.Trim()}\", /*data*/null, true,null);");
                        }
                    }
                    else if(resourceAction.OrchestrationMethodName.HasValue())
                    {
                        sb.AppendLine($"this.executeWindowRequest(\"{resourceAction.OrchestrationMethodName}\");");
                    }
                    #endregion

                    sb.AppendLine("return true;");
                    sb.PaddingCount--;
                    sb.AppendLine("}");

                    sb.AppendLine();
                }

                sb.AppendLine("return true;");
                sb.PaddingCount--;
                sb.AppendLine("}");
                #endregion
            }

            writerContext.AddClassBody(sb.ToString());
        }

        static void WriteWorkflowFields(WriterContext writerContext)
        {
            if (writerContext.HasWorkflow)
            {
                writerContext.AddClassBody(new TypeScriptMemberInfo {Code = "executeWorkFlow: () => void;", IsField = true});
            }
        }

        static void WriteTabManagementFields(WriterContext writerContext)
        {
            if (writerContext.HasTabControl)
            {
                writerContext.AddClassBody(new TypeScriptMemberInfo {Code = "readonly onProxyDidRespond: any[] = [];", IsField = true});
                writerContext.AddClassBody(new TypeScriptMemberInfo {Code = "readonly onFillRequestFromUI: any[] = [];", IsField = true});
            }
        }
        #endregion
    }
}