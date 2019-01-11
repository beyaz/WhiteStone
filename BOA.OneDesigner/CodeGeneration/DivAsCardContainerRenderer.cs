using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class DivAsCardContainerRenderer
    {
        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo, DivAsCardContainer data)
        {
            sb.AppendLine("<div>");

            sb.PaddingCount++;

            foreach (var bCard in data.Items)
            {
                BCardRenderer.Write(sb, screenInfo, bCard);

                sb.AppendLine(String.Empty);
            }

            sb.PaddingCount--;

            sb.AppendLine("</div>");
        }
    }
}