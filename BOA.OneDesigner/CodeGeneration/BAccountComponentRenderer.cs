using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BAccountComponentRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BAccountComponent data)
        {
            SnapNamingHelper.InitSnapName(data);

            sb.AppendLine($"<BAccountComponent  accountNumber = {{{data.ValueBindingPathInTypeScript}}}");
            sb.AppendLine($"                   onAccountSelect = {{(selectedAccount: any) => {{{data.ValueBindingPathInTypeScript}}} = selectedAccount ? selectedAccount.accountNumber : null}}");
            sb.AppendLine("                  isVisibleBalance = {false}");
            sb.AppendLine("            isVisibleAccountSuffix = {false}");
            sb.AppendLine("enableShowDialogMessagesInCallback = {false}");
            sb.AppendLine("                     isVisibleIBAN = {false}");
            sb.AppendLine("                               ref = {(r: any) => this.snaps."+data.SnapName+" = r}");
            sb.AppendLine("                           context = {context}/>");
        }
    }
}