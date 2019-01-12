using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BDataGridRenderer
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BDataGrid data)
        {
            SnapNamingHelper.InitSnapName(data);

            sb.AppendLine($"<BDataGrid  dataSource = {{{data.BindingPathInTypeScript}}}");

            sb.AppendLine("  selectable={'single'}");
            sb.AppendLine("                               ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("columns = {this.getGridColumns(request)}");
            sb.AppendLine("onRowSelectionChanged={this.onRowSelectionChanged}");

            sb.AppendLine("headerBarOptions={{");

            sb.PaddingCount++;

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.TitleInfo);
            if (labelValue != null)
            {
                sb.Append("showTitle:true,");
                sb.Append($"title = {{{labelValue}}},");
            }

            sb.AppendLine("show: true,");
            sb.AppendLine("showMoreOptions: true,");
            sb.AppendLine("showFiltering: true,");
            sb.AppendLine("showGrouping: true");
            sb.PaddingCount--;
            sb.AppendLine("}}");

            sb.AppendLine("context = {context}/>");
        }
        #endregion
    }
}