using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
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

            if (data.CardRefNumberBindingPath.HasValue())
            {
                var jsBindingPathCardRefNumber = new JsBindingPathInfo(data.CardRefNumberBindingPath);
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPathCardRefNumber);
                writerContext.PushVariablesToRenderScope(jsBindingPathCardRefNumber);
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoCreditCardComponent
                {
                    JsPropertyName = "cardRefNumber",
                    JsBindingPath  = jsBindingPathCardRefNumber.FullBindingPathInJs,
                    SnapName       = data.SnapName
                });
            }

            if (data.ShadowCardNumberBindingPath.HasValue())
            {
                var jsBindingPathCardRefNumber = new JsBindingPathInfo(data.ShadowCardNumberBindingPath);
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPathCardRefNumber);
                writerContext.PushVariablesToRenderScope(jsBindingPathCardRefNumber);
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoCreditCardComponent
                {
                    JsPropertyName = "shadowCardNumber",
                    JsBindingPath  = jsBindingPathCardRefNumber.FullBindingPathInJs,
                    SnapName       = data.SnapName
                });
            }

            

            sb.AppendLine("<BCreditCardComponent");
            sb.PaddingCount++;

            if (data.ValueBindingPath.HasValue())
            {
                var jsBindingPath = new JsBindingPathInfo(data.ValueBindingPath)
                {
                    EvaluateInsStateVersion = true
                };
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
                writerContext.PushVariablesToRenderScope(jsBindingPath);
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoCreditCardComponent
                {
                    JsPropertyName = "clearCardNumber",
                    JsBindingPath  = jsBindingPath.FullBindingPathInJs,
                    SnapName       = data.SnapName
                });

                sb.AppendLine($"clearCardNumber={{{jsBindingPath.FullBindingPathInJs}||\"\"}}");

                var code = $"this.syncCardComponent(this.snaps.{data.SnapName}, {RenderHelper.ConvertBindingPathToIncomingRequest(data.ValueBindingPathInTypeScript)});";

                writerContext.AddToBeforeSetStateOnProxyDidResponse(code);

                writerContext.Support_syncCardComponent();
            }

            if (data.AccountNumberBindingPath.HasValue())
            {
                var jsBindingPath = new JsBindingPathInfo(data.AccountNumberBindingPath);
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
                writerContext.PushVariablesToRenderScope(jsBindingPath);
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoCreditCardComponent
                {
                    JsPropertyName = "accountNumber",
                    JsBindingPath  = jsBindingPath.FullBindingPathInJs,
                    SnapName       = data.SnapName
                });

                sb.AppendLine($"accountNumber = {{{jsBindingPath.FullBindingPathInJs}}}");
            }

            if (data.ValueChangedOrchestrationMethod.HasValue())
            {
                sb.AppendLine("onCardSelect={(clearCardNumber: string) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"{writerContext.ExecuteWindowRequestFunctionAccessPath}(\"{data.ValueChangedOrchestrationMethod}\");");

                sb.PaddingCount--;
                sb.AppendLine("}}");
            }

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            RenderHelper.WriteSize(data.SizeInfo, sb.AppendLine);

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}