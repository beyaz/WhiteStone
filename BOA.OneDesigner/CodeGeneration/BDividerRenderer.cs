using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BDividerRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BDivider } from \"b-divider\"");

            var sb = writerContext.Output;

            sb.Append("<BDivider ");

            if (data.SizeInfo.HasValue())
            {
                sb.Append("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }
            
            sb.Append(" />");
            sb.AppendLine();
        }
        #endregion
    }
}