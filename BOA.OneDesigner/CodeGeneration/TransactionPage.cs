using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;
using WhiteStone.UI.Container;

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
                ConstructorBody = new List<string>(),
                ClassBody       = new List<TypeScriptMemberInfo>(),
                Page            = new List<string>(),
                Imports = new List<string>
                {
                    "import * as React from \"react\"",
                    "import { BFormManager } from \"b-form-manager\"",
                    "import { getMessage } from \"b-framework\"",
                    "import { ComponentSize } from \"b-component\""
                },
                ClassName    = className,
                HasWorkflow  = hasWorkflow,
                ScreenInfo   = screenInfo,
                IsBrowsePage = isBrowseForm,
                SolutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName)
            };

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
                writerContext.DataContractAccessPathInWindowRequest = "data";
            }
            else if (writerContext.RequestIntellisenseData.RequestPropertyIntellisense.Contains("DataContract"))
            {
                writerContext.DataContractAccessPathInWindowRequest = "dataContract";
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

            writerContext.EvaluatedActions = resourceActions.Where(x => x.IsVisibleBindingPath.HasValue() ||  x.IsEnableBindingPath.HasValue()).ToList();
            if (writerContext.EvaluatedActions.Count == 0)
            {
                writerContext.CanWriteEvaluateActions = false;
                return;
            }

            writerContext.CanWriteEvaluateActions = true;
        }

        static void ComponentDidMount(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine("/**");
            sb.AppendLine("  *  Components the did mount.");
            sb.AppendLine("  */");
            sb.AppendLine("componentDidMount()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super.componentDidMount();");

            sb.AppendLine();
            sb.AppendLine("if (this.state.$isLoaded)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("return;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("// Evaluates initial states of form.");
            sb.AppendLine("// Invokes 'LoadData' metod in Orchestration class.");
            sb.AppendLine();
            sb.AppendLine("const clonedWindowRequest: any = Object.assign({}, this.getWindowRequest().body); ");

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("const hasWorkflow = clonedWindowRequest.workFlowInternalData && clonedWindowRequest.workFlowInternalData.instanceId > 0;");
                sb.AppendLine("if (hasWorkflow)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine("clonedWindowRequest.hasWorkflow = false;");
                sb.AppendLine("this.sendRequestToServer(clonedWindowRequest, \"LoadData\");");
                sb.AppendLine("return;");
                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("const formData = this.state.pageParams.data;");

            sb.AppendLine("if (formData != null)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine($"clonedWindowRequest.{writerContext.DataContractAccessPathInWindowRequest} = formData;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("this.sendRequestToServer(clonedWindowRequest, \"LoadData\");");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void EvaluateActions(WriterContext writerContext)
        {
            if (writerContext.EvaluatedActions.Count == 0)
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


                    var bindingPath = TypescriptNaming.NormalizeBindingPath(Config.Value + resourceAction.IsVisibleBindingPath);

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


                    var bindingPath = TypescriptNaming.NormalizeBindingPath(Config.Value + resourceAction.IsEnableBindingPath);

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

            sb.AppendLine("/**");
            sb.AppendLine("  *  Sends given requests to server.");
            sb.AppendLine("  */");
            sb.AppendLine("executeWindowRequest(orchestrationMethodName: string)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const windowRequest = this.state.windowRequest;");
            sb.AppendLine();
            sb.AppendLine("// form should be re render because form component value changes must be handled");

            sb.AppendLine("this.updateState(() =>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const clonedWindowRequest: any = Object.assign({}, windowRequest);");

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
            sb.AppendLine("});");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void ProxyDidRespond(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine("/**");
            sb.AppendLine("  *  Proxies the did respond.");
            sb.AppendLine("  */");
            sb.AppendLine("proxyDidRespond(proxyResponse: ProxyResponse)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("let isSuccess: boolean = true;");

            sb.AppendLine();
            sb.AppendLine("if (!proxyResponse.response.success)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("isSuccess = false;");
            sb.AppendLine();
            sb.AppendLine("const results = proxyResponse.response.results;");
            sb.AppendLine();
            sb.AppendLine("if (results == null)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("this.showStatusMessage(\"DeveloperError: Genellikle type dll'inin 'd:\\boa\\one\\' folderında olmamasından kaynaklı olabilir. Build eventleri check edin. Check request headers from Developer Tools -> Network tab.\");");
            sb.AppendLine("return false;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const businessResult = results.find(r => r.severity === BOA.Common.Types.Severity.BusinessError);");
            sb.AppendLine("if (businessResult != undefined)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("this.showStatusMessage(businessResult.errorMessage);");
            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("BFormManager.showStatusErrorMessage(`Beklenmedik bir hata oluştu.${JSON.stringify(results, null, 2)}`, []);");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const incomingRequest = (proxyResponse.response as any).value;");
            sb.AppendLine("if (incomingRequest == null)");
            sb.AppendLine("{");
            sb.AppendLine("    throw new Error(`Orch method:${proxyResponse.key} should return GenericResponse<" + writerContext.ScreenInfo.RequestName + ">`);");
            sb.AppendLine("}");

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("const hasWorkflow = incomingRequest.workFlowInternalData && incomingRequest.workFlowInternalData.instanceId > 0;");
                sb.AppendLine("if (hasWorkflow)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine("const windowRequestInForm = this.getWindowRequest().body;");
                sb.AppendLine();
                sb.AppendLine("incomingRequest.hasWorkflow = windowRequestInForm.hasWorkflow;");
                sb.AppendLine();
                sb.AppendLine("if (incomingRequest.methodName === proxyResponse.key)");
                sb.AppendLine("{");
                sb.AppendLine("    incomingRequest.methodName = windowRequestInForm.methodName;");
                sb.AppendLine("}");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("const state =");

            
            writerContext.StateObjectWhenIncomingRequestIsSuccess.Add("windowRequest","incomingRequest");
            writerContext.StateObjectWhenIncomingRequestIsSuccess.Add("$isLoaded","true");
            
            JsObjectInfoMultiLineWriter.Write(sb,writerContext.StateObjectWhenIncomingRequestIsSuccess);
            sb.Append(";");
            sb.Append(Environment.NewLine);
            
            

            sb.AppendLine();
            sb.AppendLine("if (incomingRequest.statusMessage)");
            sb.AppendLine("{");
            sb.AppendLine("    BFormManager.showStatusMessage(incomingRequest.statusMessage);");
            sb.AppendLine("    incomingRequest.statusMessage = null;");
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("this.setWindowRequest(");
            sb.AppendLine("{");
            sb.AppendLine("    body: incomingRequest,");
            sb.AppendLine($"    type:\"{writerContext.ScreenInfo.RequestName}\"");
            sb.AppendLine("});");
            sb.AppendLine();
            sb.AppendLine("this.setState(state);");

            if (writerContext.CanWriteEvaluateActions)
            {
                sb.AppendLine();
                sb.AppendLine("this.evaluateActions();");
            }

            if (writerContext.HasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("if (isSuccess && this.executeWorkFlow)");
                sb.AppendLine("{");
                sb.AppendLine("    this.executeWorkFlow();");
                sb.AppendLine("}");
            }

            sb.AppendLine();
            sb.AppendLine("return isSuccess;");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        /// <summary>
        ///     Renders the specified writer context.
        /// </summary>
        static void Render(WriterContext writerContext, DivAsCardContainer jsxModel)
        {
            var temp = writerContext.Output;

            var sb = new PaddedStringBuilder();
            sb.AppendLine("/**");
            sb.AppendLine("  *  Renders the component.");
            sb.AppendLine("  */");

            sb.AppendLine("render()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (!this.state.$isLoaded)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("return <div/>;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const context = this.state.context;");

            sb.AppendLine("const request = this.state.windowRequest;");

            var sb2 = new PaddedStringBuilder {PaddingCount = sb.PaddingCount};

            sb2.AppendLine("return (");
            sb2.PaddingCount++;
            writerContext.Output = sb2;
            DivAsCardContainerRenderer.Write(writerContext, jsxModel);
            sb2.PaddingCount--;
            sb2.AppendLine(");");

            sb2.PaddingCount--;
            sb2.AppendLine("}");

            if (writerContext.BeforeRenderReturn?.Count > 0)
            {
                sb.AppendLine();
                foreach (var line in writerContext.BeforeRenderReturn)
                {
                    sb.AppendLine(line);
                }

                sb.AppendLine();
            }

            writerContext.Output = temp;

            writerContext.AddClassBody(new TypeScriptMemberInfo {Code = sb + sb2.ToString(), IsRender = true});
        }

        static void SendWindowRequestToServer(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("/**");
            sb.AppendLine("  *  Sends given requests to server.");
            sb.AppendLine("  */");
            sb.AppendLine("sendRequestToServer(request: any, orchestrationMethodName: string)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("request.methodName = orchestrationMethodName;");

            sb.AppendLine("const proxyRequest:any =");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"requestClass:\"{writerContext.ScreenInfo.RequestName}\",");
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

        static void UpdateState(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("/**");
            sb.AppendLine("  *  Clones window request object and sets states with this new cloned window request object.");
            sb.AppendLine("  */");
            sb.AppendLine("updateState(callback?: () => void)");
            sb.AppendLine("{");
            sb.AppendLine("    this.setState({ windowRequest: Object.assign({}, this.state.windowRequest) }, callback);");
            sb.AppendLine("}");

            writerContext.AddClassBody(sb.ToString());
        }

        static void WriteClass(WriterContext writerContext, DivAsCardContainer jsxModel)
        {
            var sb = new PaddedStringBuilder();

            CalculateEvaluatedActionStates(writerContext);
            CalculateDataField(writerContext);

            sb.AppendLine("/**");
            sb.AppendLine("  *  The " + string.Join(" ", FileNamingHelper.GetWords(writerContext.ClassName)));
            sb.AppendLine("  */");

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
            ComponentDidMount(writerContext);
            
            EvaluateActions(writerContext);
            UpdateState(writerContext);
            SendWindowRequestToServer(writerContext);
            ExecuteWindowRequest(writerContext);
            Render(writerContext, jsxModel);
            ProxyDidRespond(writerContext);

            WriteConstructor(writerContext);

            // reorder
            writerContext.ClassBody.Sort(TypeScriptMemberInfo.Compare);

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

            sb.AppendLine("/**");
            sb.AppendLine("  *  Creates a new instance of this class.");
            sb.AppendLine("  */");
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
            

            var resourceActions = writerContext.ScreenInfo.ResourceActions;

            var sb = new PaddedStringBuilder();
            if (writerContext.HasWorkflow)
            {
                #region onActionClick
                sb.AppendLine("/**");
                sb.AppendLine("  *  Handle click actions of page commands.");
                sb.AppendLine("  */");
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
                    
                    #region action body
                    if (resourceAction.OpenFormWithResourceCode.HasValue() && resourceAction.OrchestrationMethodName.HasValue())
                    {
                        throw Error.InvalidOperation("'Open Form With Resource Code' ve 'Orchestration Method Name' aynı anda dolu olamaz.");
                    }

                    if (resourceAction.OpenFormWithResourceCode.IsNullOrWhiteSpace() && resourceAction.OrchestrationMethodName.IsNullOrWhiteSpace())
                    {
                        throw Error.InvalidOperation("'Open Form With Resource Code' veya 'Orchestration Method Name' dan biri dolu olmalıdır.");
                    }

                    if (resourceAction.OpenFormWithResourceCode.HasValue())
                    {
                        if (resourceAction.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
                        {
                            var bindingPathForDataParameter = "this.getWindowRequest().body."+TypescriptNaming.NormalizeBindingPath(resourceAction.OpenFormWithResourceCodeDataParameterBindingPath);

                            sb.AppendLine($"BFormManager.show(\"{resourceAction.OpenFormWithResourceCode.Trim()}\", /*data*/{bindingPathForDataParameter}, true,null);");
                        }
                        else
                        {
                            sb.AppendLine($"BFormManager.show(\"{resourceAction.OpenFormWithResourceCode.Trim()}\", /*data*/null, true,null);");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"this.executeWindowRequest(\"{resourceAction.OrchestrationMethodName}\");");
                    } 
                    #endregion


                    sb.AppendLine("return /*isCompleted*/false;");
                    sb.PaddingCount--;
                    sb.AppendLine("}");

                    sb.AppendLine();
                }

                

                sb.PaddingCount--;
                sb.AppendLine("}");
                #endregion
            }
            else
            {
                #region onActionClick
                sb.AppendLine("/**");
                sb.AppendLine("  *  Handle click actions of page commands.");
                sb.AppendLine("  */");
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
                        throw Error.InvalidOperation("'Open Form With Resource Code' ve 'Orchestration Method Name' aynı anda dolu olamaz."+resourceAction.CommandName);
                    }

                    if (resourceAction.OpenFormWithResourceCode.IsNullOrWhiteSpace() && resourceAction.OrchestrationMethodName.IsNullOrWhiteSpace())
                    {
                        throw Error.InvalidOperation("'Open Form With Resource Code' veya 'Orchestration Method Name' dan biri dolu olmalıdır."+resourceAction.CommandName);
                    }

                    if (resourceAction.OpenFormWithResourceCode.HasValue())
                    {
                        if (resourceAction.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
                        {
                            var bindingPathForDataParameter = "this.getWindowRequest().body."+TypescriptNaming.NormalizeBindingPath(resourceAction.OpenFormWithResourceCodeDataParameterBindingPath);

                            sb.AppendLine($"BFormManager.show(\"{resourceAction.OpenFormWithResourceCode.Trim()}\", /*data*/{bindingPathForDataParameter}, true,null);");
                        }
                        else
                        {
                            sb.AppendLine($"BFormManager.show(\"{resourceAction.OpenFormWithResourceCode.Trim()}\", /*data*/null, true,null);");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"this.executeWindowRequest(\"{resourceAction.OrchestrationMethodName}\");");
                    } 
                    #endregion


                    sb.AppendLine("return /*isCompleted*/true;");
                    sb.PaddingCount--;
                    sb.AppendLine("}");

                    sb.AppendLine();
                }


                sb.AppendLine("return /*isCompleted*/true;");
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
        #endregion
    }
}