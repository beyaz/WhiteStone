using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BBranchComponentRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BBranchComponent data)
        {
            SnapNamingHelper.InitSnapName(data);

            // TODO fix here
            // RenderHelper.NormalizeBindingPathInRenderMethod(writerContext,data.ValueBindingPath)
            TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.ValueBindingPath);

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