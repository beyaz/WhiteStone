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

            sb.Append("context={context}");

            sb.AppendLine(" />");
        }
        #endregion
    }

    static class BButtonRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BButton } from \"b-button\"");

            var sb = writerContext.Output;

            sb.Append("<BButton type=\"flat\" colorType=\"primary\"  style = {{ float:\"right\" }}");

            RenderHelper.WriteLabelInfo(writerContext, data.TextInto,sb.Append," text");

            sb.Append("context={context}");

            sb.AppendLine(" />");
        }
        #endregion
    }


    
}