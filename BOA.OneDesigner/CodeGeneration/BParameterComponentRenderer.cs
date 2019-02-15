using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BParameterComponentRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            writerContext.Imports.Add("import { BParameterComponent } from \"b-parameter-component\"");

            SnapNamingHelper.InitSnapName(writerContext,data);

            var bindingPathInJs = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.ValueBindingPath);

            sb.AppendLine($"<BParameterComponent  selectedParamCode = {{{bindingPathInJs}+\"\"}}");
            sb.PaddingCount++;
            sb.AppendLine($" onParameterSelect = {{(selectedParameter: BOA.Types.Kernel.General.ParameterContract) => {{{bindingPathInJs}}} = selectedParameter ? selectedParameter.paramCode : null}}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");
            sb.AppendLine("paramType = \"" + data.ParamType + "\"");

            var labelValue = RenderHelper.GetLabelValue(writerContext, data.LabelTextInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"hintText = {labelValue}");
                sb.AppendLine($"labelText = {labelValue}");
            }

            if (data.IsAllOptionIncluded)
            {
                sb.AppendLine("isAllOptionIncluded={true}");
            }
            else
            {
                sb.AppendLine("isAllOptionIncluded={false}");
            }

            sb.AppendLine("paramColumns={[");
            //sb.AppendLine("{ name: \"paramCode\",        header: Message.Code,        visible: false },");
            sb.AppendLine("{ name: \"paramDescription\", header: 'Açıklama',  width:   200 }");
            sb.AppendLine("]}");



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