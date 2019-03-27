using System;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
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

        public bool OpenFormWithResourceCodeIsInDialogBox { get; set; }
        public string OrchestrationMethodOnDialogResponseIsOK { get; set; }
        public WriterContext WriterContext { get; set; }
        public string CssOfDialog { get; set; }
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

        public static BindingPathPropertyInfo GetBindingPathPropertyInfo(WriterContext writerContext, string bindingPathInDesigner)
        {
            var screenInfo         = writerContext.ScreenInfo;
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
                var jsBindingPath = new JsBindingPathCalculatorData(writerContext, data.RequestBindingPath)
                {
                    EvaluateInsStateVersion = false
                };
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

                var propertyInfo = GetBindingPathPropertyInfo(writerContext, data.RequestBindingPath);
                if (propertyInfo == null)
                {
                    return jsBindingPath.BindingPathInJs;
                }

                if (propertyInfo.IsString )
                {
                    return jsBindingPath.BindingPathInJs;
                }

                if (propertyInfo.IsNullableNumber  || 
                    propertyInfo.IsNonNullableNumber||
                    propertyInfo.IsDecimalNullable||
                    propertyInfo.IsDecimal)
                {
                    return jsBindingPath.BindingPathInJs + " || " + '"' + '"';
                }

                if (propertyInfo.IsBoolean  )
                {
                    return jsBindingPath.BindingPathInJs + " ? 'Evet' : 'Hayır'";
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

        public static void WriteButtonAction(PaddedStringBuilder sb, ButtonActionInfo buttonActionInfo)
        {
            var orchestrationMethodName = buttonActionInfo.OrchestrationMethodName;
            var resourceCode            = buttonActionInfo.OpenFormWithResourceCode;
            var openFormWithResourceCodeIsInDialogBox = buttonActionInfo.OpenFormWithResourceCodeIsInDialogBox;
            var orchestrationMethodOnDialogResponseIsOk = buttonActionInfo.OrchestrationMethodOnDialogResponseIsOK;


            var writerContext = buttonActionInfo.WriterContext;

            string dataParameter = null;

            if (buttonActionInfo.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
            {
                dataParameter = "this.snaps.data.windowRequest." + TypescriptNaming.NormalizeBindingPath(buttonActionInfo.OpenFormWithResourceCodeDataParameterBindingPath);
            }


            if (resourceCode.HasValue() && orchestrationMethodName.HasValue())
            {
                sb.AppendLine("const me: any = this;");
                sb.AppendLine("me.internalProxyDidRespondCallback = () =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                if (dataParameter.HasValue())
                {
                    sb.AppendLine("const data:any = "+ dataParameter +";");
                }
                else
                {
                    sb.AppendLine("const data:any = null;");
                }

                if (openFormWithResourceCodeIsInDialogBox)
                {
                    sb.AppendLine();
                    if (orchestrationMethodOnDialogResponseIsOk.HasValue())
                    {
                        sb.AppendLine("const onClose: any = (dialogResponse:number) =>");
                        sb.AppendLine("{");
                        sb.AppendLine("    if(dialogResponse === 1)");
                        sb.AppendLine("    {");
                        sb.AppendLine($"        this.executeWindowRequest(\"{orchestrationMethodOnDialogResponseIsOk}\");");
                        sb.AppendLine("    }");
                        sb.AppendLine("};");
                    }
                    else
                    {
                        sb.AppendLine("const onClose: any = null;");  
                    }

                    sb.AppendLine();
                    sb.AppendLine("const style:any = "+ buttonActionInfo.CssOfDialog +";");
                }


                if (openFormWithResourceCodeIsInDialogBox)
                {
                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.showDialog(\"{resourceCode}\", data, /*title*/null, onClose, style );");
                        
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine("const showAsNewPage:boolean = true;");
                    sb.AppendLine();
                    sb.AppendLine("const menuItemSuffix:string = null;");
                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.show(\"{resourceCode}\", data, showAsNewPage,menuItemSuffix);");    
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine($"this.executeWindowRequest(\"{orchestrationMethodName}\");");

                writerContext.HandleProxyDidRespondCallback = true;
            }
            else if (resourceCode.HasValue())
            {
                if (dataParameter.HasValue())
                {
                    sb.AppendLine($"BFormManager.show(\"{resourceCode}\", /*data*/{dataParameter}, true,null);");
                }
                else
                {
                    sb.AppendLine($"BFormManager.show(\"{resourceCode}\", /*data*/null, true,null);");
                }
            }
            else if (orchestrationMethodName.HasValue())
            {
                sb.AppendLine($"this.executeWindowRequest(\"{orchestrationMethodName}\");");
            }
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

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, isDisabledBindingPath)
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

        public static void WriteSize(SizeInfo sizeInfo, Action<string> output)
        {
            if (sizeInfo.HasValue())
            {
                output("size = {" + GetJsValue(sizeInfo) + "}");
            }
        }
        #endregion
    }
}