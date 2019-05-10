using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class ExcelBrowserRenderer
    {
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            SnapNamingHelper.InitSnapName(writerContext, data);

            var jsBindingPath = new JsBindingPathInfo(data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            writerContext.GrabValuesToRequest(new ComponentGetValueInfoExcelBrowser
            {
                JsBindingPath = jsBindingPath.FullBindingPathInJs,
                SnapName = data.SnapName
            });

            writerContext.Imports.Add("import { BExcelBrowser } from \"b-excel-browser\"");

            sb.AppendLine("<BExcelBrowser");

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "floatingLabelText");
            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "hintText");

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("valueType={\"99\"} // ALL_TYPE");
            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;

        }
    }
}