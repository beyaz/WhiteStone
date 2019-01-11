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
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BInput data)
        {
            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.BindingPath);

            var isDecimal = CecilHelper.FullNameOfNullableDecimal == propertyDefinition.PropertyType.FullName ||
                            propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;

            var isString = propertyDefinition.PropertyType.FullName == typeof(string).FullName;

            var bindingPathInJs = TypescriptNaming.NormalizeBindingPath($"request.{data.BindingPath}");

            if (isString)
            {
                sb.AppendLine($"<BInput value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: string) => {bindingPathInJs} = value}}");
            }
            else if (isDecimal)
            {
                sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("format = {\"D\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else
            {
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
                sb.AppendLine($"isVisible = {{{TypescriptNaming.NormalizeBindingPath("request." + data.IsVisibleBindingPath)}}}");
            }

            if (!String.IsNullOrWhiteSpace(data.IsDisabledBindingPath))
            {
                sb.AppendLine($"disabled = {{{TypescriptNaming.NormalizeBindingPath("request." + data.IsDisabledBindingPath)}}}");
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