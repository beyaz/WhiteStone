using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInputRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            if (data.ValueBindingPath.IsNullOrWhiteSpace())
            {
                throw Error.InvalidBindingPath((data.Container as BCard)?.Title, data.LabelText);
            }

            if (data.IsEnabledBindingPath.HasValue() && data.IsDisabledBindingPath.HasValue())
            {
                throw Error.InvalidOperation("Aynı anda hem enable hem disable propertsini veremezsiniz. LocationInfo-> @Input: " + data.LabelTextInfo.GetDesignerText());
            }

            if (data.ValueBindingPath == "?")
            {
                data.ValueBindingPath = "$";
            }

            SnapNamingHelper.InitSnapName(writerContext, data);


            var hasSupportErrorText = writerContext.HasSupportErrorText;

            

            var isString          = true;
            var isDecimal         = false;
            var isDecimalNullable = false;
            var isNullableNumber  = false;
            var isBoolean         = false;
            var isDateTime        = false;
            var isNullableDateTime = false;

            var jsBindingPath = new JsBindingPathInfo(data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            

            var bindingPathPropertyInfo = writerContext.ScreenInfo.GetBindingPathPropertyInfo(data.ValueBindingPath);

            if (bindingPathPropertyInfo != null)
            {
                isString = bindingPathPropertyInfo.IsString;

                isDecimal         = bindingPathPropertyInfo.IsDecimal;
                isDecimalNullable = bindingPathPropertyInfo.IsDecimalNullable;

                isNullableNumber = bindingPathPropertyInfo.IsNullableNumber;

                isBoolean = bindingPathPropertyInfo.IsBoolean;

                isDateTime = bindingPathPropertyInfo.IsDateTime;

                isNullableDateTime = bindingPathPropertyInfo.IsNullableDateTime;
            }

            var fullBindingPathInJs = jsBindingPath.FullBindingPathInJs;

            if (isString)
            {
                var tag = "BInput";
                if (data.Mask.HasValue())
                {
                    writerContext.Imports.Add("import { BInputMask } from \"b-input-mask\"");
                    tag = "BInputMask";

                    writerContext.GrabValuesToRequest(new ComponentGetValueInfoInputMask { JsBindingPath = fullBindingPathInJs,SnapName = data.SnapName});
                    
                }
                else
                {
                    writerContext.Imports.Add("import { BInput } from \"b-input\"");
                    writerContext.GrabValuesToRequest(new ComponentGetValueInfoInput { JsBindingPath = fullBindingPathInJs,SnapName = data.SnapName});
                }

                sb.AppendLine($"<{tag} value = {{{fullBindingPathInJs} === undefined ? null : {fullBindingPathInJs}}}");
                sb.PaddingCount++;

                // sb.AppendLine($"onChange = {{(e: any, value: string) => {jsBindingPath.BindingPathInJs} = value}}");

                if (data.Mask.HasValue())
                {
                    sb.AppendLine($"mask = \"{data.Mask}\"");
                }

                if (data.RowCount > 0)
                {
                    sb.AppendLine("type={\"textarea\"}");
                    sb.AppendLine("multiLine={true}");
                    sb.AppendLine("rows={" + data.RowCount + "}");
                }

                if (data.MaxLength > 0)
                {
                    sb.AppendLine("maxLength={" + data.MaxLength + "}");
                }
            }
            else if (isDecimal || isDecimalNullable)
            {
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoInput { JsBindingPath = fullBindingPathInJs,SnapName = data.SnapName});
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");

                if (isDecimal)
                {
                    sb.AppendLine($"<BInputNumeric value = {{{fullBindingPathInJs}}}");
                }
                else
                {
                    sb.AppendLine($"<BInputNumeric value = {{{fullBindingPathInJs} === undefined ? null : {fullBindingPathInJs}}}");
                }

                sb.PaddingCount++;

                // sb.AppendLine($"onChange = {{(e: any, value: number) => {jsBindingPath.BindingPathInJs} = value}}");
                sb.AppendLine("format = {\"M\"}");
                sb.AppendLine("type = {\"decimal\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else if (isBoolean)
            {
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoInput { JsBindingPath = fullBindingPathInJs,SnapName = data.SnapName});

                writerContext.Imports.Add("import { BCheckBox } from \"b-check-box\"");

                sb.AppendLine($"<BCheckBox checked = {{{fullBindingPathInJs}}}");

                hasSupportErrorText = false;

                sb.PaddingCount++;
                sb.AppendLine("verticalAlign={\"middle\"}");
                var shouldWriteOnSelectEvent = data.ValueChangedOrchestrationMethod.HasValue();
                if (shouldWriteOnSelectEvent)
                {
                    sb.AppendLine("onCheck = {(e: any, value: boolean) =>");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    
                    sb.AppendLine($"{writerContext.ExecuteWindowRequestFunctionAccessPath}(\"{data.ValueChangedOrchestrationMethod}\");");

                    sb.PaddingCount--;
                    sb.AppendLine("}}");
                }
            }
            else if (isDateTime)
            {
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoInput { JsBindingPath = fullBindingPathInJs,SnapName = data.SnapName});

                writerContext.Imports.Add("import { BDateTimePicker } from \"b-datetime-picker\"");

                if (isNullableDateTime)
                {
                    sb.AppendLine($"<BDateTimePicker value = {{{fullBindingPathInJs} || null}}");
                }
                else
                {
                    sb.AppendLine($"<BDateTimePicker value = {{{fullBindingPathInJs}}}");
                }
                
                sb.PaddingCount++;

                // sb.AppendLine($"dateOnChange = {{(e: any, value: Date) => {jsBindingPath.BindingPathInJs} = value}}");
                sb.AppendLine("format = \"DDMMYYYY\"");
            }
            else
            {
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoInput { JsBindingPath = fullBindingPathInJs,SnapName = data.SnapName});
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");

                if (isNullableNumber)
                {
                    sb.AppendLine($"<BInputNumeric value = {{{fullBindingPathInJs} === undefined ? null : {fullBindingPathInJs}}}");
                }
                else
                {
                    sb.AppendLine($"<BInputNumeric value = {{{fullBindingPathInJs} === undefined ? null : {fullBindingPathInJs}}}");
                }

                sb.PaddingCount++;

                // sb.AppendLine($"onChange = {{(e: any, value: number) => {jsBindingPath.BindingPathInJs} = value}}");
                sb.AppendLine("maxLength = {10}");
            }

            if (isBoolean)
            {
                RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "label");
            }
            else if (isDateTime)
            {
                RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "floatingLabelTextDate");
            }
            else
            {
                RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "floatingLabelText");
            }

            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);

            if (data.IsEnabledBindingPath.HasValue())
            {
                RenderHelper.WriteBooleanReverse(writerContext,"disabled",data.IsEnabledBindingPath,sb.AppendLine);    
            }

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");


            if (hasSupportErrorText)
            {
                RenderHelper.WriteErrorTextProperty(sb,fullBindingPathInJs);

                sb.AppendLine("onChange={() =>");
                sb.AppendLine("{");
                sb.AppendLine($"    {Config.ClearErrorTextMethodPathInJs}(\"{fullBindingPathInJs}\");");
                sb.AppendLine("}}");
            }


            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}