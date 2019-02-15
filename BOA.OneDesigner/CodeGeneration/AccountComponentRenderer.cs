using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class AccountComponentRenderer
    {
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

        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb         = writerContext.Output;

            SnapNamingHelper.InitSnapName(writerContext,data);

            writerContext.Imports.Add("import { BAccountComponent } from \"b-account-component\"");

            var bindingPathInJs = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.ValueBindingPath);

            writerContext.AddToBeforeSetStateOnProxyDidResponse(GetAccountComponentValueCorrection(data.SnapName, data.ValueBindingPathInTypeScript));




            sb.AppendLine($"<BAccountComponent accountNumber = {{{bindingPathInJs}}}");
            sb.PaddingCount++;

            if (data.ValueChangedOrchestrationMethod.HasValue())
            {
                sb.AppendLine("onAccountSelect = {{(selectedAccount: any) =>");    
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{bindingPathInJs} = selectedAccount ? selectedAccount.accountNumber : null;");
                sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                sb.PaddingCount--;
                sb.AppendLine("}}");    
            }
            else
            {
                sb.AppendLine($"onAccountSelect = {{(selectedAccount: any) => {bindingPathInJs} = selectedAccount ? selectedAccount.accountNumber : null}}");    
            }
                
            sb.AppendLine("isVisibleBalance={false}");
            sb.AppendLine("isVisibleAccountSuffix={false}");
            // sb.AppendLine("enableShowDialogMessagesInCallback={false}");
            sb.AppendLine("isVisibleIBAN={false}");





           
            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}