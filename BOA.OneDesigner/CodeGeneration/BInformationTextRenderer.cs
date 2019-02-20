using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInformationTextRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            writerContext.Imports.Add("import { BInformationText } from \"b-information-text\"");

            sb.AppendWithPadding("<BInformationText");

            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.Append, " labelText");
            RenderHelper.WriteLabelInfo(writerContext, data.InfoText, sb.Append, " infoText");

            sb.Append(" context = {context} />");
            sb.AppendLine();
        }
        #endregion
    }
}