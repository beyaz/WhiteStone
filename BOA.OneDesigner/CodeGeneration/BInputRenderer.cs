﻿using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInputRenderer
    {
        public static void Write(WriterContext writerContext ,BInput data)
        {

            var        sb         = writerContext.Output;
            ScreenInfo screenInfo = writerContext.ScreenInfo;

            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.BindingPath);

            var isDecimal = CecilHelper.FullNameOfNullableDecimal == propertyDefinition.PropertyType.FullName ||
                            propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;

            var isString = propertyDefinition.PropertyType.FullName == typeof(string).FullName;

            var bindingPathInJs = data.BindingPathInTypeScript;

            if (isString)
            {

                writerContext.Imports.Add("import { BInput } from \"b-input\"");

                sb.AppendLine($"<BInput value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: string) => {bindingPathInJs} = value}}");

                if (data.Mask.HasValue())
                {
                    sb.AppendLine($"mask = \"{data.Mask}\"");    
                }
            }
            else if (isDecimal)
            {
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");

                sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("format = {\"D\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else
            {
                writerContext.Imports.Add("import { BInputNumeric } from \"b-input-numeric\";");
                sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("maxLength = {10}");
            }

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"floatingLabelText = {labelValue}");
            }

            if (!String.IsNullOrWhiteSpace(data.IsVisibleBindingPath))
            {
                sb.AppendLine($"isVisible = {{{TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + data.IsVisibleBindingPath)}}}");
            }

            if (!String.IsNullOrWhiteSpace(data.IsDisabledBindingPath))
            {
                sb.AppendLine($"disabled = {{{TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + data.IsDisabledBindingPath)}}}");
            }

            if (data.SizeInfo != null && data.SizeInfo.IsEmpty == false)
            {
                sb.AppendLine("size = {"+ RenderHelper.GetJsValue(data.SizeInfo) +"}");
            }

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
    }
}