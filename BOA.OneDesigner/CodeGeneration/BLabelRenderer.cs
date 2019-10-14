using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BLabelRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BLabel } from \"b-label\"");

            var sb = writerContext.Output;

            sb.Append("<BLabel");

            RenderHelper.WriteLabelInfo(writerContext, data.TextInto,sb.Append," text");

            if (data.IsBold)
            {
                sb.Append(" style = {{ fontWeight:\"bold\" }}");
            }

            if (data.SizeInfo.HasValue())
            {
                sb.Append(" size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            sb.Append(" context={context}");

            sb.AppendLine(" />");
        }
        #endregion
    }
}