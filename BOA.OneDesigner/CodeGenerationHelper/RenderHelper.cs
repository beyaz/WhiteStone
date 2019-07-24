using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGenerationHelper
{
    static class RenderHelper
    {
        public static void InitLabelValues(WriterContext writerContext, ActionInfo actionInfo)
        {
            actionInfo.DialogTitle   = RenderHelper.GetLabelValue(writerContext, actionInfo.DialogTitleInfo);
            actionInfo.YesNoQuestion = RenderHelper.GetLabelValue(writerContext, actionInfo.YesNoQuestionInfo);

            if (actionInfo.YesNoQuestionInfo.HasValue() && 
                actionInfo.YesNoQuestionInfo.IsRequestBindingPath && 
                actionInfo.YesNoQuestion.StartsWith(Config.BindingPrefixInJs,StringComparison.OrdinalIgnoreCase))
            {
                actionInfo.YesNoQuestion = actionInfo.YesNoQuestion.Replace(Config.BindingPrefixInJs, "this.state.windowRequest.");
            }

            if (actionInfo.DialogTitleInfo.HasValue()&&
                actionInfo.DialogTitleInfo.IsRequestBindingPath && 
                actionInfo.DialogTitle.StartsWith(Config.BindingPrefixInJs,StringComparison.OrdinalIgnoreCase))
            {
                actionInfo.DialogTitle = actionInfo.DialogTitle.Replace(Config.BindingPrefixInJs, "this.state.windowRequest.");
            }
        }

        #region Public Properties
        public static bool IsCommentEnabled => false;
        #endregion

        #region Public Methods
        public static string ConvertBindingPathToIncomingRequest(string bindingPathInJs)
        {
            return bindingPathInJs.Replace(Config.BindingPrefixInJs, Config.IncomingRequestVariableName + ".");
        }

        public static BindingPathPropertyInfo GetBindingPathPropertyInfo(this ScreenInfo screenInfo, string bindingPathInDesigner)
        {
            var solutionInfo       = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);
            var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, bindingPathInDesigner);

            if (propertyDefinition == null)
            {
                return null;
            }

            var typeFullName = propertyDefinition.PropertyType.FullName;

            var isString = typeFullName == typeof(string).FullName;

            var returnValue = new BindingPathPropertyInfo
            {
                
                IsString          = isString,
                IsDecimal         = typeFullName == typeof(decimal).FullName,
                IsDecimalNullable = CecilHelper.FullNameOfNullableDecimal == typeFullName,
                IsNullableNumber = CecilHelper.FullNameOfNullableByte == typeFullName ||
                                   CecilHelper.FullNameOfNullableInt == typeFullName ||
                                   CecilHelper.FullNameOfNullableLong == typeFullName ||
                                   CecilHelper.FullNameOfNullableSbyte == typeFullName ||
                                   CecilHelper.FullNameOfNullableShort == typeFullName,

                IsNonNullableNumber = typeFullName == typeof(sbyte).FullName ||
                                      typeFullName == typeof(byte).FullName ||
                                      typeFullName == typeof(short).FullName ||
                                      typeFullName == typeof(int).FullName ||
                                      typeFullName == typeof(long).FullName ||
                                      typeFullName == typeof(decimal).FullName,

                IsBoolean = CecilHelper.FullNameOfNullableBoolean == typeFullName ||
                            typeFullName == typeof(bool).FullName,

                IsDateTime = CecilHelper.FullNameOfNullableDateTime == typeFullName ||
                             typeFullName == typeof(DateTime).FullName,

                IsNullableDateTime = CecilHelper.FullNameOfNullableDateTime == typeFullName,

                IsValueType = propertyDefinition.PropertyType.IsValueType
            };

            return returnValue;
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
                var jsBindingPath = new JsBindingPathInfo(data.RequestBindingPath)
                {
                    EvaluateInsStateVersion = false
                };
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
                writerContext.PushVariablesToRenderScope(jsBindingPath);

                var propertyInfo = GetBindingPathPropertyInfo(writerContext.ScreenInfo, data.RequestBindingPath);
                if (propertyInfo == null)
                {
                    return jsBindingPath.FullBindingPathInJs;
                }

                if (propertyInfo.IsString)
                {
                    return jsBindingPath.FullBindingPathInJs;
                }

                if (propertyInfo.IsDateTime)
                {
                    writerContext.SupportDateFormat();
                    return $"this.formatDate({jsBindingPath.FullBindingPathInJs},'{data.DateFormat}')";
                }

                if (propertyInfo.IsNullableNumber ||
                    propertyInfo.IsNonNullableNumber ||
                    propertyInfo.IsDecimalNullable ||
                    propertyInfo.IsDecimal)
                {
                    return jsBindingPath.FullBindingPathInJs + " || " + '"' + '"';
                }

                if (propertyInfo.IsBoolean)
                {
                    return jsBindingPath.FullBindingPathInJs + " ? 'Evet' : 'Hayır'";
                }

                throw Error.InvalidBindingPath(null, data.RequestBindingPath);
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

        public static void WriteIsDisabled(WriterContext writerContext, string isDisabledBindingPath, PaddedStringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(isDisabledBindingPath))
            {
                return;
            }

            var isAlwaysDisabled = string.Equals("TRUE", isDisabledBindingPath.Trim(), StringComparison.OrdinalIgnoreCase);
            if (isAlwaysDisabled)
            {
                sb.AppendLine("disabled = {true}");
                return;
            }

            var jsBindingPath = new JsBindingPathInfo(isDisabledBindingPath)
            {
                EvaluateInsStateVersion = false
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            sb.AppendLine($"disabled = {{{jsBindingPath.FullBindingPathInJs}}}");
        }

        public static void WriteIsVisible(WriterContext writerContext, string IsVisibleBindingPath, PaddedStringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(IsVisibleBindingPath))
            {
                return;
            }

            var jsBindingPath = new JsBindingPathInfo(IsVisibleBindingPath)
            {
                EvaluateInsStateVersion = false
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            sb.AppendLine($"isVisible = {{{jsBindingPath.FullBindingPathInJs}}}");
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

        public static void WriteSize(SizeInfo sizeInfo, Action<string> output)
        {
            if (sizeInfo.HasValue())
            {
                output("size = {" + GetJsValue(sizeInfo) + "}");
            }
        }
        #endregion

        public static void WriteErrorTextProperty(PaddedStringBuilder sb, string fullBindingPathInJs)
        {
            sb.AppendLine($"errorText = {{ {Config.ErrorTextPathInJs} && {Config.ErrorTextPathInJs}[\"{fullBindingPathInJs}\"] }}");
        }
    }
}