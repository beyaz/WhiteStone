using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class AccountComponentRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            SnapNamingHelper.InitSnapName(writerContext, data);

            writerContext.Imports.Add("import { BAccountComponent } from \"b-account-component\"");

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            writerContext.AddToBeforeSetStateOnProxyDidResponse(GetAccountComponentValueCorrection(data.SnapName, data.ValueBindingPathInTypeScript));

            sb.AppendLine($"<BAccountComponent accountNumber = {{{jsBindingPath.BindingPathInJsInState}}}");
            sb.PaddingCount++;

            if (data.ValueChangedOrchestrationMethod.HasValue())
            {
                sb.AppendLine("onAccountSelect = {{(selectedAccount: any) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{jsBindingPath.BindingPathInJs} = selectedAccount ? selectedAccount.accountNumber : null;");
                sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                sb.PaddingCount--;
                sb.AppendLine("}}");
            }
            else
            {
                sb.AppendLine($"onAccountSelect = {{(selectedAccount: any) => {jsBindingPath.BindingPathInJs} = selectedAccount ? selectedAccount.accountNumber : null}}");
            }

            sb.AppendLine("isVisibleBalance={false}");
            sb.AppendLine("isVisibleAccountSuffix={false}");
            // sb.AppendLine("enableShowDialogMessagesInCallback={false}");
            sb.AppendLine("isVisibleIBAN={false}");

            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);
            RenderHelper.WriteSize(data.SizeInfo,sb.AppendLine);

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion

        #region Methods
        static string GetAccountComponentValueCorrection(string snapName, string bindingPathInJs)
        {
            bindingPathInJs = RenderHelper.ConvertBindingPathToIncomingRequest(bindingPathInJs);

            var sb = new PaddedStringBuilder();

            sb.AppendLine($"if (this.snaps.{snapName} && 0 === ({bindingPathInJs}|0))");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"this.snaps.{snapName}.resetValue();");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}