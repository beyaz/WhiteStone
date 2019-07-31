﻿using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class PosMerchantComponentRenderer
    {
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            SnapNamingHelper.InitSnapName(writerContext, data);

            var jsBindingPath = new JsBindingPathInfo(data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            var fullBindingPathInJs = jsBindingPath.FullBindingPathInJs;

            writerContext.GrabValuesToRequest(new ComponentGetValueInfoPosMerchantComponent
            {
                
                JsBindingPath            = fullBindingPathInJs,
                SnapName                 = data.SnapName
            });

            writerContext.Imports.Add("import { BPosMerchantComponent } from \"b-pos-merchant-component\"");

            sb.AppendLine("<BPosMerchantComponent");
            sb.PaddingCount++;

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "floatingLabelText");
            // RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "hintText");
            

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine($"merchantNumber = {{{fullBindingPathInJs}}}");

            sb.AppendLine("dialogTitle = {\"Üye İşyeri Arama\"}");
            

            var hasSupportErrorText = writerContext.HasSupportErrorText;

            var shouldWriteOnSelectEvent = data.ValueChangedOrchestrationMethod.HasValue() || hasSupportErrorText;
            if (shouldWriteOnSelectEvent)
            {
                sb.AppendLine("onMerchantSelect = {() =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                if (hasSupportErrorText)
                {
                    sb.AppendLine($"{Config.ClearErrorTextMethodPathInJs}(\"{fullBindingPathInJs}\");");
                }

                if (data.ValueChangedOrchestrationMethod.HasValue())
                {
                    sb.AppendLine($"{writerContext.ExecuteWindowRequestFunctionAccessPath}(\"{data.ValueChangedOrchestrationMethod}\");");
                }

                sb.PaddingCount--;
                sb.AppendLine("}}");
            }

            if (hasSupportErrorText)
            {
                RenderHelper.WriteErrorTextProperty(sb,fullBindingPathInJs);
            }


            RenderHelper.WriteBoolean(writerContext, "isVisibleChainMerchantFlag", data.IsChainMerchantVisible, sb);
            RenderHelper.WriteBoolean(writerContext, "isVisibleParentMerchantFlag", data.IsParentMerchantVisible, sb);
            RenderHelper.WriteBoolean(writerContext, "disabledChainMerchantFlag", data.IsChainMerchantDisabled, sb);
            RenderHelper.WriteBoolean(writerContext, "disabledParentMerchantFlag", data.IsParentMerchantDisabled, sb);
            RenderHelper.WriteBoolean(writerContext, "checkedChainMerchantFlag", data.CheckChainMerchant, sb);
            RenderHelper.WriteBoolean(writerContext, "checkedParentMerchantFlag", data.CheckParentMerchant, sb);

           
            





            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;

        }

        
    }
}