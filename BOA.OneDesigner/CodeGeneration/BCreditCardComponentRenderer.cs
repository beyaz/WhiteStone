using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    class BCreditCardComponentRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;


            writerContext.Imports.Add("import { BCreditCardComponent } from \"b-credit-card-component\"");

            SnapNamingHelper.InitSnapName(writerContext, data);

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            sb.AppendLine("<BCreditCardComponent");
            sb.PaddingCount++;


            sb.AppendLine("onCardSelect={(selectedIndexes: any[], selectedItems: any[], selectedValues: any[]) =>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (selectedValues && selectedValues.length === 1)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine($"{jsBindingPath.BindingPathInJs} = selectedValues[0];");
            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine($"{jsBindingPath.BindingPathInJs} = null;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            if (data.ValueChangedOrchestrationMethod.HasValue())
            {
                sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
            }

            sb.PaddingCount--;
            sb.AppendLine("}}");


            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");
            
            RenderHelper.WriteSize(data.SizeInfo,sb.AppendLine);

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}