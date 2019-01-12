using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BDateTimePickerRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BDateTimePicker data)
        {
            SnapNamingHelper.InitSnapName(data);

            sb.AppendLine($"<BDateTimePicker  value = {{{data.BindingPathInTypeScript}}}");
            sb.AppendLine($"            dateOnChange = {{(e: any, value: Date) => {{{data.BindingPathInTypeScript}}} = value}}");
            sb.AppendLine("             ref = {(r: any) => this.snaps."+data.SnapName+" = r}");
            sb.AppendLine("format = \"DDMMYYYY\"");
            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"floatingLabelTextDate = {labelValue}");
            }

            sb.AppendLine("             context = {context}/>");
        }
    }
}