using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInformationTextRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            writerContext.Imports.Add("import { BInformationText } from \"b-information-text\"");

            var bindingPathInJs = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.ValueBindingPath);

            sb.AppendWithPadding("<BInformationText");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelTextInfo);
            if (labelValue != null)
            {
                sb.Append($" labelText = {labelValue}");
            }

            var infoTextValue = RenderHelper.GetLabelValue(screenInfo, data.InfoText);
            if (infoTextValue != null)
            {
                sb.Append($" infoText = {infoTextValue}");
            }

            sb.Append(" context = {context} />");
            sb.AppendLine();

            
        }
        #endregion
    }
}