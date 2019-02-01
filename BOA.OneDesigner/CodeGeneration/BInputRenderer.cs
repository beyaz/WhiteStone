using System;
using System.Windows;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BInputRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, BInput data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;


            if (data.ValueBindingPath.IsNullOrWhiteSpace())
            {
                throw Error.InvalidBindingPath((data.Container as BCard)?.Title, data.Label);
            }

            SnapNamingHelper.InitSnapName(writerContext,data);


            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var isString  = true;
            var isDecimal = false;
            var isBoolean = false;
            var isDateTime = false;

            var bindingPathInJs = data.ValueBindingPathInTypeScript;

            if (data.IsAccountComponent)
            {

                writerContext.AddBeforeRenderReturn($"if (this.snaps.{data.SnapName} && 0 === ({bindingPathInJs}|0))"+
                                                    Environment.NewLine+
                                                    "    {"+
                                                    Environment.NewLine+
                                                    $"        this.snaps.{data.SnapName}.resetValue();"+
                                                    Environment.NewLine+
                                                    "    }"
                                                    );

                writerContext.Imports.Add("import { BAccountComponent } from \"b-account-component\"");
                sb.AppendLine($"<BAccountComponent accountNumber = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                if (data.ValueChangedOrchestrationMethod.HasValue())
                {
                        sb.AppendLine($"onAccountSelect = {{(selectedAccount: any) =>");    
                        sb.AppendLine("{");
                        sb.PaddingCount++;
                        sb.AppendLine($"{bindingPathInJs} = selectedAccount ? selectedAccount.accountNumber : null;");
                        sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                        sb.PaddingCount--;
                        sb.AppendLine("}}");    
                }
                else
                {
                    sb.AppendLine($"onAccountSelect = {{(selectedAccount: any) => {bindingPathInJs} = selectedAccount ? selectedAccount.accountNumber : null}}");    
                }
                
                sb.AppendLine("isVisibleBalance={false}");
                sb.AppendLine("isVisibleAccountSuffix={false}");
                // sb.AppendLine("enableShowDialogMessagesInCallback={false}");
                sb.AppendLine("isVisibleIBAN={false}");
            }
            else
            {
                var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.ValueBindingPath);

                if (propertyDefinition != null)
                {
                    isDecimal = CecilHelper.FullNameOfNullableDecimal == propertyDefinition.PropertyType.FullName ||
                                propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;

                    isString = propertyDefinition.PropertyType.FullName == typeof(string).FullName;

                    isBoolean = CecilHelper.FullNameOfNullableBoolean == propertyDefinition.PropertyType.FullName ||
                                propertyDefinition.PropertyType.FullName == typeof(bool).FullName;


                    isDateTime = CecilHelper.FullNameOfNullableDateTime == propertyDefinition.PropertyType.FullName ||
                                propertyDefinition.PropertyType.FullName == typeof(DateTime).FullName;
                }

                if (isString)
                {

                    var tag = "BInput";
                    if (data.Mask.HasValue())
                    {
                        writerContext.Imports.Add("import { BInputMask } from \"b-input-mask\"");
                        tag = "BInputMask";
                    }
                    else
                    {
                        writerContext.Imports.Add("import { BInput } from \"b-input\"");
                    }

                 

                    sb.AppendLine($"<{tag} value = {{{bindingPathInJs}}}");
                    sb.PaddingCount++;

                    sb.AppendLine($"onChange = {{(e: any, value: string) => {bindingPathInJs} = value}}");

                    if (data.Mask.HasValue())
                    {
                        sb.AppendLine($"mask = \"{data.Mask}\"");
                    }

                    if (data.RowCount>0)
                    {
                        sb.AppendLine("type={\"textarea\"}");
                        sb.AppendLine("multiLine={true}");
                        sb.AppendLine("rows={"+data.RowCount+"}");
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
                else if (isBoolean)
                {
                    writerContext.Imports.Add("import { BCheckBox } from \"b-check-box\"");

                    sb.AppendLine($"<BCheckBox checked = {{{bindingPathInJs}}}");
                    sb.PaddingCount++;

                    sb.AppendLine($"onCheck = {{(e: any, value: boolean) => {bindingPathInJs} = value}}");
                }
                else if (isDateTime)
                {
                    writerContext.Imports.Add("import { BDateTimePicker } from \"b-datetime-picker\"");

                    sb.AppendLine($"<BDateTimePicker value = {{{bindingPathInJs}}}");
                    sb.PaddingCount++;

                    sb.AppendLine($"dateOnChange = {{(e: any, value: Date) => {bindingPathInJs} = value}}");
                    sb.AppendLine("format = \"DDMMYYYY\"");
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
                    if (isBoolean)
                    {
                        sb.AppendLine($"label = {{{labelValue}}}");
                    }
                    else if (isDateTime)
                    {
                        sb.AppendLine($"floatingLabelTextDate = {{{labelValue}}}");
                    }
                    else
                    {
                        sb.AppendLine($"floatingLabelText = {{{labelValue}}}");
                    }
                }

                
            }

            

            if (!string.IsNullOrWhiteSpace(data.IsVisibleBindingPath))
            {
                sb.AppendLine($"isVisible = {{{TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.IsVisibleBindingPath)}}}");
            }

            if (!string.IsNullOrWhiteSpace(data.IsDisabledBindingPath))
            {
                sb.AppendLine($"disabled = {{{TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.IsDisabledBindingPath)}}}");
            }

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {" + RenderHelper.GetJsValue(data.SizeInfo) + "}");
            }

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}