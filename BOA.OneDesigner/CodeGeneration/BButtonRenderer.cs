using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BButtonRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            writerContext.Imports.Add("import { BButton } from \"b-button\"");

            var sb = writerContext.Output;

            sb.AppendLine("<BButton");
            sb.PaddingCount++;
            if (data.ButtonTypeIsRaised)
            {
                sb.AppendLine(" type=\"raised\"");    
            }
            else
            {
                sb.AppendLine(" type=\"flat\"");
            }
            
            sb.AppendLine(" colorType=\"primary\"");
            

            sb.AppendLine("style = {{ float:\"right\" }}");

            if (data.IsEnabledBindingPath.HasValue() && data.IsDisabledBindingPath.HasValue())
            {
                throw Error.InvalidOperation("Aynı anda hem enable hem disable propertsini veremezsiniz. LocationInfo-> @ButtonLabel: " + data.TextInto.GetDesignerText());
            }

            RenderHelper.WriteLabelInfo(writerContext, data.TextInto, sb.AppendLine, " text");
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);
            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);

            if (data.IsEnabledBindingPath.HasValue())
            {
                RenderHelper.WriteBooleanReverse(writerContext,"disabled",data.IsEnabledBindingPath,sb.AppendLine);    
            }
            

            sb.AppendLine("onClick={()=>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            RenderHelper.InitLabelValues(writerContext, data.ButtonClickedActionInfo);

            var function = new ActionInfoFunction
            {
                Data = data.ButtonClickedActionInfo,
                WriterContext = writerContext
            };

            sb.AppendAll(function.GetCode());
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("}}");

            sb.AppendLine("context={context} />");

            sb.PaddingCount--;
            
        }
        #endregion
    }
}