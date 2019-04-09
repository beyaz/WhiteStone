using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    class ButtonActionInfoFunction
    {
        #region Public Properties
        public ButtonActionInfo Data          { get; set; }
        public WriterContext    WriterContext { get; set; }
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
                if (WriterContext.IsTabPage)
                {
                    sb.AppendLine($"Extension.{Data.ExtensionMethodName}(this.state.pageInstance);");
                }
                else
                {
                    sb.AppendLine($"Extension.{Data.ExtensionMethodName}(this);");
                }

                return sb.ToString();
            }

            var executeWindowRequest = WriterContext.ExecuteWindowRequestFunctionAccessPath;

            string dataParameter = null;


            if (Data.OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
            {
                dataParameter = "this.state.windowRequest." + TypescriptNaming.NormalizeBindingPath(Data.OpenFormWithResourceCodeDataParameterBindingPath);
            }

            if (Data.OpenFormWithResourceCode.HasValue() && Data.OrchestrationMethodName.HasValue())
            {
                sb.AppendLine($"{mainFormPath}.internalProxyDidRespondCallback = () =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                

                if (dataParameter.HasValue())
                {
                    sb.AppendLine("const data: any = " + dataParameter + ";");
                }
                else
                {
                    sb.AppendLine("const data:any = null;");
                }

                var onCloseText = "/*onClose*/null";

                if (Data.OpenFormWithResourceCodeIsInDialogBox)
                {
                    

                    
                    if (Data.OrchestrationMethodOnDialogResponseIsOK.HasValue())
                    {
                        if (dataParameter.HasValue())
                        {
                            sb.AppendLine();
                            sb.AppendLine("const onClose: any = (dialogResponse:number, returnValue: any) =>");
                            sb.AppendLine("{");
                            sb.AppendLine("    if(dialogResponse === 1)");
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
                            sb.AppendLine("    if(dialogResponse === 1)");
                            sb.AppendLine("    {");
                            sb.AppendLine($"        {executeWindowRequest}(\"{Data.OrchestrationMethodOnDialogResponseIsOK}\");");
                            sb.AppendLine("    }");
                            sb.AppendLine("};");
                        }
                        

                        onCloseText = "onClose";
                    }

                    sb.AppendLine();
                    sb.AppendLine("const style:any = " + Data.CssOfDialog + ";");
                }

                

                if (Data.OpenFormWithResourceCodeIsInDialogBox)
                {
                    var titleText = "/*title*/null";
                    if (Data.Title.HasValue())
                    {
                        sb.AppendLine();
                        sb.AppendLine("const title = " + Data.Title+ ";");

                        titleText = "title";
                    }

                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.showDialog(\"{Data.OpenFormWithResourceCode}\", data, {titleText}, {onCloseText}, style );");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine("const showAsNewPage:boolean = true;");
                    sb.AppendLine();
                    sb.AppendLine("const menuItemSuffix:string = null;");
                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.show(\"{Data.OpenFormWithResourceCode}\", data, showAsNewPage,menuItemSuffix);");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine($"{executeWindowRequest}(\"{Data.OrchestrationMethodName}\");");

                WriterContext.HandleProxyDidRespondCallback = true;
            }
            else if (Data.OpenFormWithResourceCode.HasValue())
            {
                if (dataParameter.HasValue())
                {
                    sb.AppendLine($"BFormManager.show(\"{Data.OpenFormWithResourceCode}\", /*data*/{dataParameter}, true,null);");
                }
                else
                {
                    sb.AppendLine($"BFormManager.show(\"{Data.OpenFormWithResourceCode}\", /*data*/null, true,null);");
                }
            }
            else if (Data.OrchestrationMethodName.HasValue())
            {
                sb.AppendLine($"{executeWindowRequest}(\"{Data.OrchestrationMethodName}\");");
            }

            return sb.ToString();
        }
        #endregion
    }
}