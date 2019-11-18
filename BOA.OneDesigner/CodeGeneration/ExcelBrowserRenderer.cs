using BOA.Common.Helpers;
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
                ValueBindingPathInDotNet = data.ValueBindingPath,
                JsBindingPath = jsBindingPath.FullBindingPathInJs,
                SnapName = data.SnapName
            });

            writerContext.AddToBeforeSetStateOnProxyDidResponse(GetValueCorrection(data.SnapName, data.ValueBindingPathInTypeScript));

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

            sb.AppendLine("isVisibleTable={false}");

            if (data.IsEnabledBindingPath.HasValue())
            {
                RenderHelper.WriteBooleanReverse(writerContext,"disabled",data.IsEnabledBindingPath,sb.AppendLine);    
            }

            if (data.ValueChangedOrchestrationMethod.HasValue())
            {
                sb.AppendLine("valueChange = {() =>");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{writerContext.ExecuteWindowRequestFunctionAccessPath}(\"{data.ValueChangedOrchestrationMethod}\");");
                sb.PaddingCount--;
                sb.AppendLine("}}");
            }


            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;

            writerContext.Support_excelRead();

        }

        static string GetValueCorrection(string snapName, string bindingPathInJs)
        {
            bindingPathInJs = RenderHelper.ConvertBindingPathToIncomingRequest(bindingPathInJs);

            var sb = new PaddedStringBuilder();

            sb.AppendLine($"if (this.snaps.{snapName} &&  this.readExcel(this.snaps.{snapName}).length !== ({bindingPathInJs}||[]).length)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"this.snaps.{snapName}.resetValue();");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

       
    }
}