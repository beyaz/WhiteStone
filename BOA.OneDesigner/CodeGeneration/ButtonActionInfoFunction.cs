using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    class ButtonActionInfoFunction
    {
        #region Public Properties
        public string DesignerLocation                                 { get; set; }
        public string OpenFormWithResourceCode                         { get; set; }
        public string OpenFormWithResourceCodeDataParameterBindingPath { get; set; }
        public string OrchestrationMethodName                          { get; set; }

        public bool          OpenFormWithResourceCodeIsInDialogBox   { get; set; }
        public string        OrchestrationMethodOnDialogResponseIsOK { get; set; }
        public WriterContext WriterContext                           { get; set; }
        public string        CssOfDialog                             { get; set; }
        public string ExtensionMethodName { get; set; }
        #endregion


        
        public string GetCode()
        {
            var sb = new PaddedStringBuilder();

            if (ExtensionMethodName.HasValue())
            {
                sb.AppendLine($"Extension.{ExtensionMethodName}(this);");

                return sb.ToString();
            }

            var executeWindowRequest = WriterContext.ExecuteWindowRequestFunctionAccessPath;
            

            string dataParameter = null;

            if (OpenFormWithResourceCodeDataParameterBindingPath.HasValue())
            {
                dataParameter = "this.state.windowRequest." + TypescriptNaming.NormalizeBindingPath(OpenFormWithResourceCodeDataParameterBindingPath);
            }

            if (OpenFormWithResourceCode.HasValue() && OrchestrationMethodName.HasValue())
            {
                sb.AppendLine("const me: any = this;");
                sb.AppendLine("me.internalProxyDidRespondCallback = () =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                if (dataParameter.HasValue())
                {
                    sb.AppendLine("const data:any = " + dataParameter + ";");
                }
                else
                {
                    sb.AppendLine("const data:any = null;");
                }

                if (OpenFormWithResourceCodeIsInDialogBox)
                {
                    sb.AppendLine();
                    if (OrchestrationMethodOnDialogResponseIsOK.HasValue())
                    {
                        sb.AppendLine("const onClose: any = (dialogResponse:number) =>");
                        sb.AppendLine("{");
                        sb.AppendLine("    if(dialogResponse === 1)");
                        sb.AppendLine("    {");
                        sb.AppendLine($"        {executeWindowRequest}(\"{OrchestrationMethodOnDialogResponseIsOK}\");");
                        sb.AppendLine("    }");
                        sb.AppendLine("};");
                    }
                    else
                    {
                        sb.AppendLine("const onClose: any = null;");
                    }

                    sb.AppendLine();
                    sb.AppendLine("const style:any = " + CssOfDialog + ";");
                }

                if (OpenFormWithResourceCodeIsInDialogBox)
                {
                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.showDialog(\"{OpenFormWithResourceCode}\", data, /*title*/null, onClose, style );");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine("const showAsNewPage:boolean = true;");
                    sb.AppendLine();
                    sb.AppendLine("const menuItemSuffix:string = null;");
                    sb.AppendLine();
                    sb.AppendLine($"BFormManager.show(\"{OpenFormWithResourceCode}\", data, showAsNewPage,menuItemSuffix);");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine($"{executeWindowRequest}(\"{OrchestrationMethodName}\");");

                WriterContext.HandleProxyDidRespondCallback = true;
            }
            else if (OpenFormWithResourceCode.HasValue())
            {
                if (dataParameter.HasValue())
                {
                    sb.AppendLine($"BFormManager.show(\"{OpenFormWithResourceCode}\", /*data*/{dataParameter}, true,null);");
                }
                else
                {
                    sb.AppendLine($"BFormManager.show(\"{OpenFormWithResourceCode}\", /*data*/null, true,null);");
                }
            }
            else if (OrchestrationMethodName.HasValue())
            {
                sb.AppendLine($"{executeWindowRequest}(\"{OrchestrationMethodName}\");");
            }


            return sb.ToString();
        }

    }
}