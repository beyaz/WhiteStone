using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    class ButtonActionInfo
    {
        #region Public Properties
        public string DesignerLocation                                 { get; set; }
        public string OpenFormWithResourceCode                         { get; set; }
        public string OpenFormWithResourceCodeDataParameterBindingPath { get; set; }
        public string OrchestrationMethodName                          { get; set; }
        #endregion
    }

    static class RenderHelper
    {
        #region Public Properties
        public static bool IsCommentEnabled => false;
        #endregion

        #region Public Methods
        public static string ConvertBindingPathToIncomingRequest(string bindingPathInJs)
        {
            return bindingPathInJs.Replace(Config.BindingPrefixInJs, Config.IncomingRequestVariableName + ".");
        }

        public static string GetJsValue(SizeInfo size)
        {
            if (size.IsLarge)
            {
                return "ComponentSize.LARGE";
            }

            if (size.IsMedium)
            {
                return "ComponentSize.MEDIUM";
            }

            if (size.IsSmall)
            {
                return "ComponentSize.SMALL";
            }

            if (size.IsExtraSmall)
            {
                return "ComponentSize.XSMALL";
            }

            throw Error.InvalidOperation();
        }

        public static string GetLabelValue(WriterContext writerContext, LabelInfo data)
        {
            var screenInfo = writerContext.ScreenInfo;

            if (data == null)
            {
                return null;
            }

            if (data.IsFreeText)
            {
                if (data.FreeTextValue.IsNullOrWhiteSpace())
                {
                    return null;
                }

                return '"' + data.FreeTextValue + '"';
            }

            if (data.IsRequestBindingPath)
            {
                var jsBindingPath = new JsBindingPathCalculatorData(writerContext, data.RequestBindingPath)
                {
                    EvaluateInsStateVersion = false
                };
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

                return jsBindingPath.BindingPathInJs;
            }

            if (data.IsFromMessaging)
            {
                return $"getMessage(\"{screenInfo.MessagingGroupName}\", \"{data.MessagingValue}\")";
            }

            return null;
        }

        public static bool HasValue(this SizeInfo size)
        {
            return size != null && size.IsEmpty == false;
        }

        public static string TransformBindingPathInJsToStateAccessedVersion(string bindingPathInJs)
        {
            var paths = bindingPathInJs.Split('.');
            if (paths.Length != 2)
            {
                throw Error.InvalidOperation(bindingPathInJs);
            }

            paths[0] = paths[0] + "InState";

            return string.Join(".", paths);
        }

        public static void WriteButtonAction(PaddedStringBuilder sb, ButtonActionInfo buttonActionInfo)
        {
            if (buttonActionInfo.OpenFormWithResourceCode.HasValue() && buttonActionInfo.OrchestrationMethodName.HasValue())
            {
                throw Error.InvalidOperation("'Open Form With Resource Code' ve 'Orchestration Method Name' aynı anda dolu olamaz." + buttonActionInfo.DesignerLocation);
            }

            if (buttonActionInfo.OpenFormWithResourceCode.IsNullOrWhiteSpace() && buttonActionInfo.OrchestrationMethodName.IsNullOrWhiteSpace())
            {
                throw Error.InvalidOperation("'Open Form With Resource Code' veya 'Orchestration Method Name' dan biri dolu olmalıdır." + buttonActionInfo.DesignerLocation);
            }

            if (buttonActionInfo.OpenFormWithResourceCode.HasValue())
            {
                if (buttonActionInfo.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
                {
                    var bindingPathForDataParameter = "this.getWindowRequest().body." + TypescriptNaming.NormalizeBindingPath(buttonActionInfo.OpenFormWithResourceCodeDataParameterBindingPath);

                    sb.AppendLine($"BFormManager.show(\"{buttonActionInfo.OpenFormWithResourceCode.Trim()}\", /*data*/{bindingPathForDataParameter}, true,null);");
                }
                else
                {
                    sb.AppendLine($"BFormManager.show(\"{buttonActionInfo.OpenFormWithResourceCode.Trim()}\", /*data*/null, true,null);");
                }
            }
            else
            {
                sb.AppendLine($"this.executeWindowRequest(\"{buttonActionInfo.OrchestrationMethodName}\");");
            }
        }

        public static void WriteIsDisabled(WriterContext writerContext, string IsDisabledBindingPath, PaddedStringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(IsDisabledBindingPath))
            {
                return;
            }

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, IsDisabledBindingPath)
            {
                EvaluateInsStateVersion = false
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            sb.AppendLine($"disabled = {{{jsBindingPath.BindingPathInJs}}}");
        }

        public static void WriteIsVisible(WriterContext writerContext, string IsVisibleBindingPath, PaddedStringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(IsVisibleBindingPath))
            {
                return;
            }

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, IsVisibleBindingPath)
            {
                EvaluateInsStateVersion = false
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            sb.AppendLine($"isVisible = {{{jsBindingPath.BindingPathInJs}}}");
        }

        public static void WriteLabelInfo(WriterContext writerContext, LabelInfo data, Action<string> output, string attributeName, string endPrefix = null)
        {
            var labelValue = GetLabelValue(writerContext, data);
            if (labelValue == null)
            {
                return;
            }

            if (attributeName.EndsWith(":"))
            {
                output($"{attributeName} {labelValue}" + endPrefix);
            }
            else
            {
                output($"{attributeName} = {{{labelValue}}}" + endPrefix);
            }
        }
        #endregion
    }
}