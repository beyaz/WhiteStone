using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    class WriterContext
    {
        #region Public Properties
        public string              ClassName   { get; set; }
        public bool                HasWorkflow { get; set; }
        public List<string>        Imports     { get; set; }
        public PaddedStringBuilder Output      { get; set; }
        public ScreenInfo          ScreenInfo  { get; set; }

        public List<string> PageBodies { get; set; }
        #endregion
    }

    static class TransactionPage
    {
        #region Public Methods
        public static string Generate(ScreenInfo screenInfo)
        {
            var sb       = new PaddedStringBuilder();
            var jsxModel = (DivAsCardContainer) screenInfo.JsxModel;

            var hasWorkflow = screenInfo.FormType == FormType.TransactionPageWithWorkflow;
            var className   = screenInfo.RequestName.SplitAndClear(".").Last().RemoveFromEnd("Request");

            var writerContext = new WriterContext
            {
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
                ScreenInfo  = screenInfo,
                Output      = sb
            };

            sb.AppendLine();
            sb.AppendLine($"class {className} extends TransactionPage");
            sb.AppendLine("{");
            sb.PaddingCount++;

            if (hasWorkflow)
            {
                writerContext.PageBodies.Add("executeWorkFlow: () => void;");
            }


            WriteConstructor(writerContext);
            WriteOnActionClick(writerContext);



            #region componentDidMount
            sb.AppendLine("componentDidMount()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super.componentDidMount();");
            sb.AppendLine("FormAssistant.componentDidMount(this);");

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.AppendLine();

            #region proxyDidRespond
            sb.AppendLine("proxyDidRespond(proxyResponse: ProxyResponse)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return FormAssistant.receiveResponse(this,proxyResponse);");

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.AppendLine();

            #region render
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
            DivAsCardContainerRenderer.Write(writerContext, jsxModel);
            sb.PaddingCount--;
            sb.AppendLine(");");

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();

            sb.AppendLine($"export default TransactionPageComposer({className});");


           
            


            return 
                string.Join(Environment.NewLine,writerContext.Imports.Distinct().ToList()) + Environment.NewLine + 
                sb;
        }

        static void WriteOnActionClick(WriterContext writerContext)
        {
            PaddedStringBuilder sb = new PaddedStringBuilder();
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

            writerContext.PageBodies.Add(sb.ToString());
        }

        static void WriteConstructor( WriterContext writerContext)
        {
            PaddedStringBuilder sb = new PaddedStringBuilder();

            sb.AppendLine("constructor(props: BFramework.BasePageProps)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super(props);");
            sb.AppendLine("this.connect(this);");
            sb.AppendLine($"FormAssistant.initialize(this, \"{writerContext.ScreenInfo.RequestName}\");");
            sb.PaddingCount--;
            sb.AppendLine("}");

            writerContext.PageBodies.Add(sb.ToString());
        }
        #endregion
    }
}