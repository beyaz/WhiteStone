using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;

namespace BOA.OneDesigner.CodeGeneration
{
    class FillWindowRequestFunctionDefinition
    {
        readonly IReadOnlyList<ComponentGetValueInfo> FillRequestFromUI;

        public FillWindowRequestFunctionDefinition(IReadOnlyList<ComponentGetValueInfo> fillRequestFromUi)
        {
            FillRequestFromUI = fillRequestFromUi;
        }

        public string GetFunction()
        {
            var sb = new PaddedStringBuilder();

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Fills given requests from ui values.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("fillRequestFromUI(request: any)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const snaps = this.snaps;");
            



            var map = new Dictionary<string,bool>();

            var sameValuesArrayIsDefined = false;

            foreach (var data in FillRequestFromUI)
            {
                if (map.ContainsKey(data.JsBindingPath))
                {
                    continue;
                }

                var sameTarget = FillRequestFromUI.Where(x=>x.JsBindingPath == data.JsBindingPath).ToList();
                if (sameTarget.Count>1)
                {

                    if (!sameValuesArrayIsDefined)
                    {
                        sameValuesArrayIsDefined = true;

                        sb.AppendLine();
                        sb.AppendLine("const sameValues: number[] = [];");
                    }
                    else
                    {
                        sb.AppendLine();
                        sb.AppendLine("sameValues.length = 0;");
                    }

                    sb.AppendLine();
                    sb.AppendLine($"// #region {data.JsBindingPath}");
                    

                    foreach (var item in sameTarget)
                    {
                        sb.AppendLine($"if (snaps.{item.SnapName})");
                        sb.AppendLine("{");
                        sb.PaddingCount++;

                        sb.AppendLine($"{item.JsBindingPath} = {item.GetCode()};"); 
                        sb.AppendLine("sameValues.push(0);"); 
                        sb.PaddingCount--;
                        sb.AppendLine("}");
                    }

                    sb.AppendLine("if (sameValues.length > 1)");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine($"const ErrorMessage = \"{data.JsBindingPath} alanı aynı anda birden fazla componentden değer alamaz.\";");
                    sb.AppendLine("BFormManager.showStatusErrorMessage(ErrorMessage,null);");
                    sb.AppendLine("throw ErrorMessage;");

                    sb.PaddingCount--;
                    sb.AppendLine("}");
                    
                    sb.AppendLine("// #endregion");

                    map[data.JsBindingPath] = true;

                    continue;

                    
                }

                sb.AppendLine();
                sb.AppendLine($"{data.JsBindingPath} = snaps.{data.SnapName} && {data.GetCode()};");   
                
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}