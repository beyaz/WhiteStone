using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
    class FillWindowRequestFunctionDefinition
    {
        #region Public Properties
        public IReadOnlyList<ComponentGetValueInfo> FillRequestFromUI { get; set; }
        public bool                                 HasTabControl     { get; set; }
        #endregion

        #region Public Methods
        public string GetCode()
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
            sb.AppendLine($"let {ComponentGetValueInfo.VariableNameOfComponent}: any = null;");

            var map = new Dictionary<string, bool>();

            var sameValuesArrayIsDefined = false;

            foreach (var data in FillRequestFromUI)
            {
                if (map.ContainsKey(data.JsBindingPath))
                {
                    continue;
                }

                var sameTarget = FillRequestFromUI.Where(x => x.JsBindingPath == data.JsBindingPath).ToList();
                if (sameTarget.Count > 1)
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
                        
                        sb.AppendLine($"{ComponentGetValueInfo.VariableNameOfComponent} = snaps.{item.SnapName};");
                        sb.AppendLine($"if ({ComponentGetValueInfo.VariableNameOfComponent})");
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

                if (data is ComponentGetValueInfoExcelBrowser excelBrowser)
                {
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    excelBrowser.Write(sb);

                    sb.PaddingCount--;
                    sb.AppendLine("}");
                    continue;
                }

                if (data is ComponentGetValueInfoDataGridSelectedValueChangedBindingValueInBrowseForm)
                {
                    sb.AppendLine();
                    sb.AppendLine($"{data.JsBindingPath} = {data.GetAssignmentValueCode()};");
                    continue;
                }

                if (data is ComponentGetValueInfoComboBox comboBox &&  comboBox.BindingPathPropertyInfo.IsNonNullableNumber)
                {
                    sb.AppendLine();

                    sb.AppendLine($"{ComponentGetValueInfo.VariableNameOfComponent} = snaps.{data.SnapName};");
                    sb.AppendLine($"if ({ComponentGetValueInfo.VariableNameOfComponent})");
                    sb.AppendLine("{");
                    sb.AppendLine($"    const cmpValue = {data.GetAssignmentValueCode()};"); 
                    sb.AppendLine("    if (cmpValue != null && cmpValue !== \"\")");
                    sb.AppendLine("    {");
                    sb.AppendLine($"        {data.JsBindingPath} = cmpValue;");
                    sb.AppendLine("    }");
                    
                    sb.AppendLine("}");

                    continue;
                }
                
                


                sb.AppendLine();

                sb.AppendLine($"{ComponentGetValueInfo.VariableNameOfComponent} = snaps.{data.SnapName};");
                sb.AppendLine($"if ({ComponentGetValueInfo.VariableNameOfComponent})");
                sb.AppendLine("{");
                sb.AppendLine($"    {data.JsBindingPath} = {data.GetAssignmentValueCode()};");
                sb.AppendLine("}");

                

                if (data is ComponentGetValueInfoAccountComponent accountComponent)
                {
                    if (accountComponent.BindPropertyTypeIsNonNullableNumber == true)
                    {
                        sb.AppendLine($"if({data.JsBindingPath} === \"\")");
                        sb.AppendLine("{");
                        sb.AppendLine($"    {data.JsBindingPath} = undefined;");
                        sb.AppendLine("}");
                    }
                    if (accountComponent.BindPropertyTypeIsNullableNumber == true)
                    {
                        sb.AppendLine($"if({data.JsBindingPath} === \"\")");
                        sb.AppendLine("{");
                        sb.AppendLine($"    {data.JsBindingPath} = null;");
                        sb.AppendLine("}");
                    }
                }
                

                if (data is ComponentGetValueInfoDataGridSelectedValueChangedBindingValue )
                {
                    var hasSameValue = FillRequestFromUI.Any(x=>x.JsBindingPath.StartsWith(data.JsBindingPath+".") && x!= data);
                    if (hasSameValue)
                    {
                        throw Error.DataGridSelectedRecordInvalidBindingPath( data.JsBindingPath);
                    }
                }
            }

            if (HasTabControl)
            {
                sb.AppendLine();
                sb.AppendLine("this.onFillRequestFromUI.forEach(tabPage => tabPage.fillRequestFromUI(request));");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}