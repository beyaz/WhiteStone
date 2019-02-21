﻿using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BButtonRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BButton } from \"b-button\"");

            var sb = writerContext.Output;

            sb.Append("<BButton type=\"flat\" colorType=\"primary\"  style = {{ float:\"right\" }}");

            RenderHelper.WriteLabelInfo(writerContext, data.TextInto, sb.Append, " text");
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);
            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);

            sb.AppendLine("onClick={()=>{");

            RenderHelper.WriteButtonAction(sb, new ButtonActionInfo
            {
                OrchestrationMethodName                          = data.ButtonClickedOrchestrationMethod,
                OpenFormWithResourceCode                         = data.OpenFormWithResourceCode,
                OpenFormWithResourceCodeDataParameterBindingPath = data.OpenFormWithResourceCodeDataParameterBindingPath,
                DesignerLocation                                 = data.Text
            });

            sb.AppendLine("}}");

            sb.Append("context={context}");

            sb.AppendLine(" />");
        }
        #endregion
    }
}