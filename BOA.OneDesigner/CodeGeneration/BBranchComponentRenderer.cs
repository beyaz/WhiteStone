using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BBranchComponentRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BBranchComponent data)
        {
            SnapNamingHelper.InitSnapName(data);

            sb.AppendLine($"<BBranchComponent  selectedBranchId = {{{data.ValueBindingPathInTypeScript}}}");
            sb.AppendLine($"                   onBranchSelect = {{(selectedBranch: BOA.Common.Types.BranchContract) => {{{data.ValueBindingPathInTypeScript}}} = selectedBranch ? selectedBranch.branchId : null}}");
            sb.AppendLine("                  mode = {\"horizontal\"}");
            sb.AppendLine("            sortOption = {BBranchComponent.name}");
            sb.AppendLine("                               ref = {(r: any) => this.snaps."+data.SnapName+" = r}");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"label = {labelValue}");
            }

            sb.AppendLine("                           context = {context}/>");
        }
    }
}