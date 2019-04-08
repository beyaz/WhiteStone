using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BButtonRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BButton } from \"b-button\"");

            var sb = writerContext.Output;

            sb.Append("<BButton type=\"flat\" colorType=\"primary\"");
            sb.PaddingCount++;

            sb.AppendLine("style = {{ float:\"right\" }}");

            RenderHelper.WriteLabelInfo(writerContext, data.TextInto, sb.Append, " text");
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);
            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);

            sb.AppendLine("onClick={()=>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            var function = new ButtonActionInfoFunction
            {
                OrchestrationMethodName                          = data.ButtonClickedOrchestrationMethod,
                OpenFormWithResourceCode                         = data.OpenFormWithResourceCode,
                OpenFormWithResourceCodeDataParameterBindingPath = data.OpenFormWithResourceCodeDataParameterBindingPath,
                DesignerLocation                                 = data.Text,
                OrchestrationMethodOnDialogResponseIsOK          = data.OrchestrationMethodOnDialogResponseIsOK,
                OpenFormWithResourceCodeIsInDialogBox            = data.OpenFormWithResourceCodeIsInDialogBox,
                CssOfDialog                                      = data.CssOfDialog,

                WriterContext = writerContext,
                ExtensionMethodName = data.ExtensionMethodName
                
            };


            sb.AppendAll(function.GetCode());

            sb.PaddingCount--;
            sb.AppendLine("}}");

            sb.Append("context={context}");

            sb.PaddingCount--;
            sb.AppendLine(" />");
        }
        #endregion
    }
}