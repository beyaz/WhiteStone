using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    class ActionInfoFunction
    {
        #region Public Properties
        public ActionInfo    Data          { get; set; }
        public WriterContext WriterContext { get; set; }
        #endregion

        #region Public Methods
        public string GetCode()
        {
            var sb = new PaddedStringBuilder();

            var mainFormPath = "this";
            if (WriterContext.IsTabPage)
            {
                mainFormPath = "this.state.pageInstance";
            }

            if (Data.ExtensionMethodName.HasValue())
            {
                sb.AppendLine($"Extension.{Data.ExtensionMethodName}({mainFormPath});");

                return sb.ToString();
            }

            var executeWindowRequest = WriterContext.ExecuteWindowRequestFunctionAccessPath;

            if (Data.OrchestrationMethodName.HasValue() &&
                Data.OpenFormWithResourceCode.IsNullOrEmpty() && Data.YesNoQuestion.IsNullOrWhiteSpace())
            {
                sb.AppendLine($"{executeWindowRequest}(\"{Data.OrchestrationMethodName}\");");
                return sb.ToString();
            }

            string dataParameter = null;
            if (Data.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
            {
                dataParameter = "this.state.windowRequest." + TypescriptNaming.NormalizeBindingPath(Data.OpenFormWithResourceCodeDataParameterBindingPath);
            }

            if (Data.OrchestrationMethodName.IsNullOrEmpty() &&
                Data.OpenFormWithResourceCode.HasValue())
            {
                if (dataParameter.HasValue())
                {
                    sb.AppendLine($"BFormManager.show(\"{Data.OpenFormWithResourceCode}\", /*data*/{dataParameter}, true,null);");
                }
                else
                {
                    sb.AppendLine($"BFormManager.show(\"{Data.OpenFormWithResourceCode}\", /*data*/null, true,null);");
                }

                return sb.ToString();
            }

            if (Data.OpenFormWithResourceCode.IsNullOrWhiteSpace() && Data.OrchestrationMethodName.IsNullOrWhiteSpace() &&
                Data.YesNoQuestion.IsNullOrWhiteSpace())
            {
                return sb.ToString();
            }

            if (Data.OrchestrationMethodName.HasValue())
            {
                sb.AppendLine();
                sb.AppendLine($"{mainFormPath}.addToProcessQueue(() =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"{mainFormPath}.executeWindowRequest(\"{Data.OrchestrationMethodName}\");");

                sb.PaddingCount--;
                sb.AppendLine("});");
            }

            #region YesNoQuestion
            if (Data.YesNoQuestion.HasValue())
            {
                sb.AppendLine();
                sb.AppendLine($"{mainFormPath}.addToProcessQueue(() =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                if (Data.YesNoQuestionCondition.HasValue())
                {
                    var jsBindingPath = new JsBindingPathInfo(Data.YesNoQuestionCondition)
                    {
                        EvaluateInsStateVersion = true
                    };
                    JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
                    WriterContext.PushVariablesToRenderScope(jsBindingPath);


                    sb.AppendLine($"if(!{jsBindingPath.FullBindingPathInJs})");
                    sb.AppendLine("{");
                    sb.AppendLine($"    {mainFormPath}.runProcessQueue();");
                    sb.AppendLine("    return;");
                    sb.AppendLine("}");
                }

                WriterContext.Imports.Add(Imports.DialogHelper);
                WriterContext.Imports.Add(Imports.DialogType);
                WriterContext.Imports.Add(Imports.DialogResponseStyle);
                WriterContext.Imports.Add(Imports.DialogResponse);

                sb.AppendLine($@"BDialogHelper.show({mainFormPath}.state.context, {Data.YesNoQuestion}, DialogType.QUESTION, DialogResponseStyle.YESNO, ""Soru"",");
                sb.PaddingCount++;
                sb.AppendLine("(dialogResponse: any) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("if (dialogResponse === DialogResponse.YES)");
                sb.AppendLine("{");
                sb.AppendLine($"    {mainFormPath}.runProcessQueue();");
                sb.AppendLine("}");
                sb.AppendLine("else");
                sb.AppendLine("{");
                sb.AppendLine($"    {mainFormPath}.processQueue.shift();");
                sb.AppendLine("}");

                sb.PaddingCount--;
                sb.AppendLine("}");

                sb.PaddingCount--;
                sb.AppendLine(");");

                sb.PaddingCount--;
                sb.AppendLine("});");


                if (Data.YesNoQuestionAfterYesOrchestrationCall.HasValue())
                {
                    sb.AppendLine();
                    sb.AppendLine($"{mainFormPath}.addToProcessQueue(() =>");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine($"{mainFormPath}.executeWindowRequest(\"{Data.YesNoQuestionAfterYesOrchestrationCall}\");");

                    sb.PaddingCount--;
                    sb.AppendLine("});");
                }
            }
            #endregion

            #region Open Form with resource code
            if (Data.OpenFormWithResourceCode.HasValue())
            {
                sb.AppendLine();
                sb.AppendLine($"{mainFormPath}.addToProcessQueue(() =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                if (Data.OpenFormWithResourceCodeCondition.HasValue())
                {
                    sb.AppendLine($"if(!{Data.OpenFormWithResourceCodeCondition})");
                    sb.AppendLine("{");
                    sb.AppendLine($"    {mainFormPath}.runProcessQueue();");
                    sb.AppendLine("    return;");
                    sb.AppendLine("}");
                }

                var dataParameterText = "/*data*/null";
                if (dataParameter.HasValue())
                {
                    sb.AppendLine("const data: any = " + dataParameter + ";");
                    dataParameterText = "data";
                }

                var onCloseText = "/*onClose*/null";

                if (Data.OpenFormWithResourceCodeIsInDialogBox)
                {
                    if (Data.OrchestrationMethodOnDialogResponseIsOK.HasValue())
                    {
                        WriterContext.Imports.Add(Imports.DialogResponse);
                        if (dataParameter.HasValue())
                        {
                            sb.AppendLine();
                            sb.AppendLine("const onClose: any = (dialogResponse:number, returnValue: any) =>");
                            sb.AppendLine("{");
                            sb.AppendLine("    if(dialogResponse === DialogResponse.OK)");
                            sb.AppendLine("    {");
                            sb.AppendLine($"        {dataParameter} = returnValue;");
                            sb.AppendLine($"        {executeWindowRequest}(\"{Data.OrchestrationMethodOnDialogResponseIsOK}\");");
                            sb.AppendLine("    }");
                            sb.AppendLine("};");
                        }
                        else
                        {
                            sb.AppendLine();
                            sb.AppendLine("const onClose: any = (dialogResponse:number) =>");
                            sb.AppendLine("{");
                            sb.AppendLine("    if(dialogResponse === DialogResponse.OK)");
                            sb.AppendLine("    {");
                            sb.AppendLine($"        {executeWindowRequest}(\"{Data.OrchestrationMethodOnDialogResponseIsOK}\");");
                            sb.AppendLine("    }");
                            sb.AppendLine("};");
                        }

                        onCloseText = "onClose";
                    }

                    sb.AppendLine();
                    sb.AppendLine("const style: any = " + Data.CssOfDialog + ";");

                    var titleText = "/*title*/null";
                    if (Data.DialogTitle.HasValue())
                    {
                        sb.AppendLine();
                        sb.AppendLine("const title = " + Data.DialogTitle + ";");

                        titleText = "title";
                    }

                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.showDialog(\"{Data.OpenFormWithResourceCode}\", {dataParameterText}, {titleText}, {onCloseText}, style );");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine("const showAsNewPage: boolean = true;");
                    sb.AppendLine();
                    sb.AppendLine("const menuItemSuffix: string = null;");
                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.show(\"{Data.OpenFormWithResourceCode}\", {dataParameterText}, showAsNewPage, menuItemSuffix);");
                }

                sb.PaddingCount--;
                sb.AppendLine("});");
            }
            #endregion

            sb.AppendLine();
            sb.AppendLine($"{mainFormPath}.runProcessQueue();");

            WriterContext.HandleProxyDidRespondCallback = true;

            return sb.ToString();
        }
        #endregion
    }
}