﻿using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BLabelRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BLabel } from \"b-label\"");

            var sb = writerContext.Output;

            sb.Append($"<BLabel");

            var textValue = RenderHelper.GetLabelValue(writerContext, data.TextInto);
            if (textValue != null)
            {
                sb.Append($" text = {{{textValue}}}");
            }

            if (data.IsBold)
            {
                sb.Append(" style = {{ fontWeight:\"bold\" }}");
            }

            sb.AppendLine(" />");
        }
        #endregion
    }
}