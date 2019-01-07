using System.Linq;
using BOA.Common.Helpers;
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

            // GeneralParametersFormRequest
            var className = screenInfo.RequestName.SplitAndClear(".").Last().RemoveFromEnd("Request");

            sb.AppendLine("import * as React from \"react\"");
            sb.AppendLine("import { TransactionPage, TransactionPageComposer } from \"b-framework\"");
            sb.AppendLine("import { BCard } from \"b-card\"");

            if (jsxModel.HasComponent<BInput>())
            {
                sb.AppendLine("import { BInput } from \"b-input\"");
            }

            sb.AppendLine(string.Empty);
            sb.AppendLine($"class {className} extends TransactionPage");
            sb.Append("{");
            sb.PaddingCount++;

            sb.AppendLine(string.Empty);

            #region constructor
            sb.Append("constructor(props: BFramework.BasePageProps)");
            sb.Append("{");
            sb.PaddingCount++;

            sb.Append("super(props);");
            sb.Append("this.connect(this);");
            sb.Append("FormAssistant.initialize(this, RequestName.GeneralParametersFormRequest);");

            sb.PaddingCount--;
            sb.Append("}");
            #endregion

            sb.AppendLine(string.Empty);

            #region onActionClick
            sb.Append("onActionClick(command: BOA.Common.Types.ResourceActionContract)");
            sb.Append("{");
            sb.PaddingCount++;

            sb.Append("FormAssistant.executeWindowRequest(this,command.commandName);");
            sb.Append("return /*isCompleted*/true;");

            sb.PaddingCount--;
            sb.Append("}");
            #endregion

            sb.AppendLine(string.Empty);

            #region componentDidMount
            sb.Append("componentDidMount()");
            sb.Append("{");
            sb.PaddingCount++;

            sb.Append("super.componentDidMount();");
            sb.Append("FormAssistant.componentDidMount(this);");

            sb.PaddingCount--;
            sb.Append("}");
            #endregion

            sb.AppendLine(string.Empty);

            #region proxyDidRespond
            sb.Append("proxyDidRespond(proxyResponse: ProxyResponse)");
            sb.Append("{");
            sb.PaddingCount++;

            sb.Append("return FormAssistant.receiveResponse(this,proxyResponse);");

            sb.PaddingCount--;
            sb.Append("}");
            #endregion

            sb.AppendLine(string.Empty);

            #region render
            sb.Append("render()");
            sb.Append("{");
            sb.PaddingCount++;

            sb.Append("if (!FormAssistant.isReadyToRender(this))");
            sb.Append("{");
            sb.PaddingCount++;
            sb.Append("return <div/>;");
            sb.PaddingCount--;
            sb.Append("}");

            sb.AppendLine(string.Empty);
            sb.Append("const context = this.state.context;");

            sb.Append("const windowRequest: GeneralParametersFormRequest = FormAssistant.getWindowRequest(this);");

            sb.Append("return (");
            sb.PaddingCount++;
            sb.Write(jsxModel);
            sb.PaddingCount--;
            sb.Append(")");

            sb.PaddingCount--;
            sb.Append("}");
            #endregion

            sb.PaddingCount--;
            sb.Append("}");

            return sb.ToString();
        }
        #endregion
    }
}