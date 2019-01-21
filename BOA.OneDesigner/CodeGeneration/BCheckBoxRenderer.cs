using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BCheckBoxRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BCheckBox data)
        {
            SnapNamingHelper.InitSnapName(data);

            sb.AppendLine($"<BCheckBox  checked = {{{data.ValueBindingPathInTypeScript}}}");
            sb.AppendLine($"            onCheck = {{(e: any, isChecked: boolean) => {{{data.ValueBindingPathInTypeScript}}} = isChecked}}");
            sb.AppendLine("             ref = {(r: any) => this.snaps."+data.SnapName+" = r}");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"label = {labelValue}");
            }

            sb.AppendLine("             context = {context}/>");
        }
    }
}