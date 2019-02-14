using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInputRenderer2
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            if (data.ValueBindingPath.IsNullOrWhiteSpace())
            {
                throw Error.InvalidBindingPath((data.Container as BCard)?.Title, data.LabelText);
            }

            SnapNamingHelper.InitSnapName(writerContext, data);

            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var isString = true;

            var isDecimal         = false;
            var isDecimalNullable = false;

            var isNullableNumber = false;

            var isBoolean = false;

            var isDateTime = false;

            var bindingPathInJs = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.ValueBindingPath);

            var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.ValueBindingPath);

            if (propertyDefinition != null)
            {
                isString = propertyDefinition.PropertyType.FullName == typeof(string).FullName;

                isDecimal         = propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;
                isDecimalNullable = CecilHelper.FullNameOfNullableDecimal == propertyDefinition.PropertyType.FullName;

                isNullableNumber = CecilHelper.FullNameOfNullableByte == propertyDefinition.PropertyType.FullName ||
                                   CecilHelper.FullNameOfNullableInt == propertyDefinition.PropertyType.FullName ||
                                   CecilHelper.FullNameOfNullableLong == propertyDefinition.PropertyType.FullName ||
                                   CecilHelper.FullNameOfNullableSbyte == propertyDefinition.PropertyType.FullName ||
                                   CecilHelper.FullNameOfNullableShort == propertyDefinition.PropertyType.FullName;

                isBoolean = CecilHelper.FullNameOfNullableBoolean == propertyDefinition.PropertyType.FullName ||
                            propertyDefinition.PropertyType.FullName == typeof(bool).FullName;

                isDateTime = CecilHelper.FullNameOfNullableDateTime == propertyDefinition.PropertyType.FullName ||
                             propertyDefinition.PropertyType.FullName == typeof(DateTime).FullName;
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

                sb.AppendLine($"<{tag} value = {{{bindingPathInJs}||\"\"}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: string) => {bindingPathInJs} = value}}");

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
                    sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                }
                else
                {
                    sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}||\"\"}}");
                }

                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("format = {\"D\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else if (isBoolean)
            {
                writerContext.Imports.Add("import { BCheckBox } from \"b-check-box\"");

                sb.AppendLine($"<BCheckBox checked = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onCheck = {{(e: any, value: boolean) => {bindingPathInJs} = value}}");
            }
            else if (isDateTime)
            {
                writerContext.Imports.Add("import { BDateTimePicker } from \"b-datetime-picker\"");

                sb.AppendLine($"<BDateTimePicker value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"dateOnChange = {{(e: any, value: Date) => {bindingPathInJs} = value}}");
                sb.AppendLine("format = \"DDMMYYYY\"");
            }
            else
            {
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");

                if (isNullableNumber)
                {
                    sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}||\"\"}}");
                }
                else
                {
                    sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                }

                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("maxLength = {10}");
            }

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelTextInfo);
            if (labelValue != null)
            {
                if (isBoolean)
                {
                    sb.AppendLine($"label = {{{labelValue}}}");
                }
                else if (isDateTime)
                {
                    sb.AppendLine($"floatingLabelTextDate = {{{labelValue}}}");
                }
                else
                {
                    sb.AppendLine($"floatingLabelText = {{{labelValue}}}");
                }
            }

            if (!string.IsNullOrWhiteSpace(data.IsVisibleBindingPath))
            {
                sb.AppendLine($"isVisible = {{{RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.IsVisibleBindingPath)}}}");
            }

            if (!string.IsNullOrWhiteSpace(data.IsDisabledBindingPath))
            {
                sb.AppendLine($"disabled = {{{RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.IsDisabledBindingPath)}}}");
            }

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