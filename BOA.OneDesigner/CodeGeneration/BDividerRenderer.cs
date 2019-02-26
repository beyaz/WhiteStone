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

            RenderHelper.WriteSize(data.SizeInfo,sb.Append);
            
            sb.Append(" />");
            sb.AppendLine();
        }
        #endregion
    }
}