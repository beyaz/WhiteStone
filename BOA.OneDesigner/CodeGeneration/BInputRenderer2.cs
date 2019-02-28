using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInputRenderer2
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            if (data.ValueBindingPath.IsNullOrWhiteSpace())
            {
                throw Error.InvalidBindingPath((data.Container as BCard)?.Title, data.LabelText);
            }

            SnapNamingHelper.InitSnapName(writerContext, data);

            var isString          = true;
            var isDecimal         = false;
            var isDecimalNullable = false;
            var isNullableNumber  = false;
            var isBoolean         = false;
            var isDateTime        = false;

            if (data.ValueBindingPath == "?")
            {
                data.ValueBindingPath = "$";
            }

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            var bindingPathPropertyInfo = RenderHelper.GetBindingPathPropertyInfo(writerContext, data.ValueBindingPath);

            if (bindingPathPropertyInfo != null)
            {
                isString = bindingPathPropertyInfo.IsString;

                isDecimal         = bindingPathPropertyInfo.IsDecimal;
                isDecimalNullable = bindingPathPropertyInfo.IsDecimalNullable;

                isNullableNumber = bindingPathPropertyInfo.IsNullableNumber;

                isBoolean = bindingPathPropertyInfo.IsBoolean;

                isDateTime = bindingPathPropertyInfo.IsDateTime;
            }

            if (isString)
            {
                var tag = "BInput";
                if (data.Mask.HasValue())
                {
                    writerContext.Imports.Add("import { BInputMask } from \"b-input-mask\"");
                    tag = "BInputMask";
                }
                else
                {
                    writerContext.Imports.Add("import { BInput } from \"b-input\"");
                }

                sb.AppendLine($"<{tag} value = {{{jsBindingPath.BindingPathInJsInState} || \"\"}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: string) => {jsBindingPath.BindingPathInJs} = value}}");

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
            }
            else if (isDecimal || isDecimalNullable)
            {
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");

                if (isDecimal)
                {
                    sb.AppendLine($"<BInputNumeric value = {{{jsBindingPath.BindingPathInJsInState}}}");
                }
                else
                {
                    sb.AppendLine($"<BInputNumeric value = {{{jsBindingPath.BindingPathInJsInState} || \"\"}}");
                }

                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {jsBindingPath.BindingPathInJs} = value}}");
                sb.AppendLine("format = {\"D\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else if (isBoolean)
            {
                writerContext.Imports.Add("import { BCheckBox } from \"b-check-box\"");

                sb.AppendLine($"<BCheckBox checked = {{{jsBindingPath.BindingPathInJsInState}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onCheck = {{(e: any, value: boolean) => {jsBindingPath.BindingPathInJs} = value}}");
            }
            else if (isDateTime)
            {
                writerContext.Imports.Add("import { BDateTimePicker } from \"b-datetime-picker\"");

                sb.AppendLine($"<BDateTimePicker value = {{{jsBindingPath.BindingPathInJsInState}}}");
                sb.PaddingCount++;

                sb.AppendLine($"dateOnChange = {{(e: any, value: Date) => {jsBindingPath.BindingPathInJs} = value}}");
                sb.AppendLine("format = \"DDMMYYYY\"");
            }
            else
            {
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");

                if (isNullableNumber)
                {
                    sb.AppendLine($"<BInputNumeric value = {{{jsBindingPath.BindingPathInJsInState} || \"\"}}");
                }
                else
                {
                    sb.AppendLine($"<BInputNumeric value = {{{jsBindingPath.BindingPathInJsInState}}}");
                }

                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {jsBindingPath.BindingPathInJs} = value}}");
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

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}