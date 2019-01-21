using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class DivAsCardContainerRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, DivAsCardContainer data)
        {
            var sb = writerContext.Output;
            sb.AppendLine("<div>");

            sb.PaddingCount++;

            foreach (var bCard in data.Items)
            {
                BCardRenderer.Write(writerContext, bCard);

                sb.AppendLine(string.Empty);

                if (writerContext.IsBrowsePage)
                {
                    // write Only Criteria columns
                    break;
                }
            }

            sb.PaddingCount--;

            sb.AppendLine("</div>");
        }
        #endregion
    }
}