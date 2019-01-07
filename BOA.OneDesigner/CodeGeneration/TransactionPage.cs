using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class TransactionPage
    {
        #region Public Methods
        public static string Generate(ScreenInfo screenInfo)
        {
            var sb       = new PaddedStringBuilder();
            var jsxModel = (DivAsCardContainer) screenInfo.JsxModel;

            var hasWorkflow = screenInfo.FormType == FormType.TransactionPageWithWorkflow;


            // GeneralParametersFormRequest
            var className = screenInfo.RequestName.SplitAndClear(".").Last().RemoveFromEnd("Request");

            sb.AppendLine("import * as React from \"react\"");
            sb.AppendLine("import { TransactionPage, TransactionPageComposer } from \"b-framework\"");
            sb.AppendLine("import { getMessage } from \"b-framework\"");

            sb.AppendLine("import { ComponentSize } from \"b-component\"");
            
            sb.AppendLine("import { FormAssistant } from \"../utils/FormAssistant\";");

            sb.AppendLine("import { BCard } from \"b-card\"");
            sb.AppendLine("import { BInputNumeric } from \"b-input-numeric\";");

            if (jsxModel.HasComponent<BInput>())
            {
                sb.AppendLine("import { BInput } from \"b-input\"");
            }

            sb.AppendLine();
            sb.AppendLine($"class {className} extends TransactionPage");
            sb.AppendLine("{");
            sb.PaddingCount++;


            if (hasWorkflow)
            {
                sb.AppendLine();
                sb.AppendLine("executeWorkFlow: () => void;");
            }


            sb.AppendLine();

            

            #region constructor
            sb.AppendLine("constructor(props: BFramework.BasePageProps)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super(props);");
            sb.AppendLine("this.connect(this);");
            sb.AppendLine($"FormAssistant.initialize(this, \"{screenInfo.RequestName}\");");
            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.AppendLine();

            if (hasWorkflow)
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
          

            sb.AppendLine();

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
            sb.Write(screenInfo,jsxModel);
            sb.PaddingCount--;
            sb.AppendLine(");");

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();

            sb.AppendLine($"export default TransactionPageComposer({className});");

            return sb.ToString();
        }
        #endregion
    }
}