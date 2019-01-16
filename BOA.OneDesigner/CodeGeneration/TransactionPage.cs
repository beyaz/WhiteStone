using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
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

            var hasWorkflow = screenInfo.FormType == FormType.TransactionPageWithWorkflow;
            var className   = screenInfo.RequestName.SplitAndClear(".").Last().RemoveFromEnd("Request");

            var writerContext = new WriterContext
            {
                ConstructorBody = new List<string>(),
                ClassBody = new List<string>(),
                Page = new List<string>(),
                Imports = new List<string>
                {
                    "import * as React from \"react\"",
                    "import { TransactionPage, TransactionPageComposer } from \"b-framework\"",
                    "import { getMessage } from \"b-framework\"",
                    "import { ComponentSize } from \"b-component\"",
                    "import { FormAssistant } from \"../utils/FormAssistant\";",
                    "import { BCard } from \"b-card\""
                },
                ClassName   = className,
                HasWorkflow = hasWorkflow,
                ScreenInfo  = screenInfo
            };

            writerContext.SolutionInfo           = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);
            writerContext.RequestIntellisenseData = CecilHelper.GetRequestIntellisenseData(writerContext.SolutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName);

            WriteClass(writerContext, jsxModel);

            writerContext.Page.Add($"export default TransactionPageComposer({className});");

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
        static void ComponentDidMount(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine("componentDidMount()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super.componentDidMount();");
            sb.AppendLine("FormAssistant.componentDidMount(this);");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.ClassBody.Add(sb.ToString());
        }

        static void ProxyDidRespond(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine("proxyDidRespond(proxyResponse: ProxyResponse)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return FormAssistant.receiveResponse(this,proxyResponse);");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.ClassBody.Add(sb.ToString());
        }

        static void EvaluateActionStates(WriterContext writerContext)
        {
            var resourceActions = writerContext.ScreenInfo.ResourceActions;
            if (resourceActions == null )
            {
                return;
            }

            resourceActions = resourceActions.Where(x => x.IsEnabledBindingPath.HasValue()).ToList();
            if ( resourceActions.Count == 0)
            {
                return;
            }

            var sb = new PaddedStringBuilder();
            sb.AppendLine("evaluateActionStates()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const request: any = FormAssistant.getWindowRequest(this);");

            sb.AppendLine();

            foreach (var resourceAction in resourceActions)
            {
                var bindingPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + resourceAction.IsEnabledBindingPath);
                sb.AppendLine($"FormAssistant.setActionState(this,\"{resourceAction.CommandName}\", {bindingPath});");
            }
           
            

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.ClassBody.Add(sb.ToString());
        }

        static void Render(WriterContext writerContext, DivAsCardContainer jsxModel)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine("render()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (!FormAssistant.isReadyToRender(this))");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine("return <div/>;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const context = this.state.context;");

            sb.AppendLine("const request: any = FormAssistant.getWindowRequest(this);");

            sb.AppendLine("return (");
            sb.PaddingCount++;
            writerContext.Output = sb;
            DivAsCardContainerRenderer.Write(writerContext, jsxModel);
            sb.PaddingCount--;
            sb.AppendLine(");");

            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.ClassBody.Add(sb.ToString());
        }

        static void WriteClass(WriterContext writerContext, DivAsCardContainer jsxModel)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"class {writerContext.ClassName} extends TransactionPage");
            sb.Append("{");
            sb.PaddingCount++;

            WriteWorkflowFields(writerContext);
            
            WriteOnActionClick(writerContext);
            ComponentDidMount(writerContext);
            ProxyDidRespond(writerContext);
            EvaluateActionStates(writerContext);
            Render(writerContext, jsxModel);
            WriteConstructor(writerContext);




            foreach (var member in writerContext.ClassBody)
            {
                sb.AppendLine();
                sb.AppendAll(member);
                sb.AppendLine();
            }

            sb.PaddingCount--;
            sb.AppendLine("}");


            writerContext.Page.Add(sb.ToString());
        }

        static void WriteConstructor(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("constructor(props: BFramework.BasePageProps)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super(props);");
            sb.AppendLine("this.connect(this);");
            sb.AppendLine($"FormAssistant.initialize(this, \"{writerContext.ScreenInfo.RequestName}\");");

            foreach (var line in writerContext.ConstructorBody)
            {
                sb.AppendLine(line);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");


            writerContext.ClassBody.Insert(1,sb.ToString());   
            
        }

        static void WriteOnActionClick(WriterContext writerContext)
        {
            var sb = new PaddedStringBuilder();
            if (writerContext.HasWorkflow)
            {
                #region onActionClick
                sb.AppendLine("onActionClick(command: BOA.Common.Types.ResourceActionContract, executeWorkFlow: () => void)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("this.executeWorkFlow = executeWorkFlow;");
                sb.AppendLine("FormAssistant.executeWindowRequest(this,command.commandName);");
                sb.AppendLine("return /*isCompleted*/false;");

                sb.PaddingCount--;
                sb.AppendLine("}");
                #endregion
            }
            else
            {
                #region onActionClick
                sb.AppendLine("onActionClick(command: BOA.Common.Types.ResourceActionContract)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("FormAssistant.executeWindowRequest(this,command.commandName);");
                sb.AppendLine("return /*isCompleted*/true;");

                sb.PaddingCount--;
                sb.AppendLine("}");
                #endregion
            }

            writerContext.ClassBody.Add(sb.ToString());
        }

        static void WriteWorkflowFields(WriterContext writerContext)
        {
            if (writerContext.HasWorkflow)
            {
                writerContext.ClassBody.Add("executeWorkFlow: () => void;");
            }
        }
        #endregion
    }
}