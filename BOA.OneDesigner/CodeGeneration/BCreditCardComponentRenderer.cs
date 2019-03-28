using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
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

            sb.AppendLine($"clearCardNumber={{{jsBindingPath.BindingPathInJsInState}}}");
            sb.AppendLine("onCardSelect={(clearCardNumber: string) =>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"{jsBindingPath.BindingPathInJs} = clearCardNumber;");

            if (data.ValueChangedOrchestrationMethod.HasValue())
            {
                sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
            }

            writerContext.GrabValuesToRequest(new ComponentGetValueInfoCreditCardComponent { JsBindingPath = jsBindingPath.FullBindingPathInJs,SnapName = data.SnapName});

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