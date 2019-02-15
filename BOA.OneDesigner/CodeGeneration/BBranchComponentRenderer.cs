using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BBranchComponentRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            SnapNamingHelper.InitSnapName(writerContext,data);

            writerContext.Imports.Add("import { BBranchComponent } from \"b-branch-component\"");

            var bindingPathInJs = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.ValueBindingPath);

            sb.AppendLine($"<BBranchComponent  selectedBranchId = {{{bindingPathInJs}}}");
            sb.PaddingCount++;

            sb.AppendLine($"onBranchSelect = {{(selectedBranch: BOA.Common.Types.BranchContract) => {{{bindingPathInJs}}} = selectedBranch ? selectedBranch.branchId : null}}");
            sb.AppendLine("mode = {\"horizontal\"}");
            sb.AppendLine("sortOption = {BBranchComponent.name}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelTextInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"label = {labelValue}");
            }


            if (!string.IsNullOrWhiteSpace(data.IsVisibleBindingPath))
            {
                sb.AppendLine($"isVisible = {{{RenderHelper.NormalizeBindingPathInRenderMethod( writerContext,data.IsVisibleBindingPath)}}}");
            }

            if (!string.IsNullOrWhiteSpace(data.IsDisabledBindingPath))
            {
                sb.AppendLine($"disabled = {{{RenderHelper.NormalizeBindingPathInRenderMethod( writerContext, data.IsDisabledBindingPath)}}}");
            }

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }


            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}