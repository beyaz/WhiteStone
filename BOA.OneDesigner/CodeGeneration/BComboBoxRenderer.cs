﻿using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.CodeGeneration
{
    internal static class BComboBoxRenderer
    {

       
        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BComboBox data)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine("/**");
            sb.AppendLine("  *  Gets the column definition of "+ data.SnapName+".");
            sb.AppendLine("  */");
            sb.AppendLine(methodName + "(request:any) : any[]");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const columns: any[] = [];");
            var isFirst = true;
            foreach (var bDataGridColumnInfo in data.DataGrid.Columns)
            {
                if (string.IsNullOrWhiteSpace(bDataGridColumnInfo.BindingPath))
                {
                    App.ShowErrorNotification($"{data.SelectedValueBindingPath} ismindeki combonun grid colonlarındaki binding path bilgisi bulunamadı.@labelValue: {bDataGridColumnInfo.LabelText}");
                    continue;
                }

                sb.AppendLine();
                sb.AppendLine("// " + bDataGridColumnInfo.BindingPath);

                if (bDataGridColumnInfo.IsVisibleBindingPath.HasValue())
                {
                    var bindingPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + bDataGridColumnInfo.IsVisibleBindingPath);
                    sb.AppendLine($"if( !{bindingPath} )");
                    sb.AppendLine("{");
                    sb.AppendLine("    continue;");
                    sb.AppendLine("}");
                }

                if (isFirst)
                {
                    isFirst = false;
                    sb.AppendLine("let column:any = {};");
                }
                else
                {
                    sb.AppendLine("column = {};");
                }

                sb.AppendLine($"column.key = \"{TypescriptNaming.NormalizeBindingPath(bDataGridColumnInfo.BindingPath)}\";");

                var labelValue = RenderHelper.GetLabelValue(writerContext.ScreenInfo, bDataGridColumnInfo.Label);
                if (labelValue != null)
                {
                    sb.AppendLine($"column.name = {labelValue};");
                }

                var propertyInfo = writerContext.RequestIntellisenseData.FindPropertyInfoInCollectionFirstGenericArgumentType(data.DataGrid.DataSourceBindingPath, bDataGridColumnInfo.BindingPath);
                if (propertyInfo == null)
                {
                    throw Error.InvalidBindingPath(data.DataGrid.DataSourceBindingPath, bDataGridColumnInfo.BindingPath);
                }

                if (propertyInfo.IsDecimal || propertyInfo.IsDecimalNullable)
                {
                    sb.AppendLine("column.type = 'number';");
                    sb.AppendLine("column.numberFormat = 'M';");
                }
                else if (propertyInfo.IsNumber)
                {
                    sb.AppendLine("column.type = 'number';");
                }
                else if (propertyInfo.IsDate || propertyInfo.IsDateNullable)
                {
                    sb.AppendLine("column.type = 'date';");
                }

                sb.AppendLine("columns.push(column);");
            }

            sb.AppendLine();
            sb.AppendLine("return columns;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        #region Public Methods

        public static void Write(WriterContext writerContext, BComboBox data)
        {
            writerContext.Imports.Add("import { BComboBox } from \"b-combo-box\"");

            var sb = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            if (data.SelectedValueBindingPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(data.SelectedValueBindingPath));
            }
            SnapNamingHelper.InitSnapName(writerContext, data);

            writerContext.AddClassBody(EvaluateMethodBodyOfGridColumns(data.TypeScriptMethodNameOfGetGridColumns, writerContext, data));

            var propertyDefinition = CecilHelper.FindPropertyInfo(writerContext.SolutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.SelectedValueBindingPath);

            if (data.IsMultiSelect)
            {
                if (CecilHelper.IsCollection(propertyDefinition?.PropertyType) == false)
                {
                    throw Error.InvalidOperation($"Is Multi Select true  ise {data.SelectedValueBindingPath} değeri collection tipinde olmalıdır.");
                }
            }
            

            var selectedValueBindingPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + data.SelectedValueBindingPath);
            var valueMemberPath = TypescriptNaming.NormalizeBindingPath(data.ValueMemberPath);
            var displayMemberPath = TypescriptNaming.NormalizeBindingPath(data.DisplayMemberPath);

            if (valueMemberPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(valueMemberPath));
            }

            if (displayMemberPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(displayMemberPath));
            }

            sb.AppendLine($"<BComboBox  dataSource = {{{data.DataGrid.DataSourceBindingPathInTypeScript}}}");
            sb.PaddingCount++;

            sb.AppendLine($"value={{[{selectedValueBindingPath}]}}");
            if (data.IsMultiSelect)
            {
                sb.AppendLine("onSelect={(selectedIndexes: any[], selectedItems: any[], selectedValues: any[]) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"{selectedValueBindingPath} = selectedValues;");

                if (data.ValueChangedOrchestrationMethod.HasValue())
                {
                    sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                }

                sb.PaddingCount--;
                sb.AppendLine("}}");
            }
            else
            {
                sb.AppendLine("onSelect={(selectedIndexes: any[], selectedItems: any[]) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("if (selectedItems && selectedItems.length === 1)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{selectedValueBindingPath} = selectedItems[0].{valueMemberPath};");
                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine("else");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{selectedValueBindingPath} = null;");
                sb.PaddingCount--;
                sb.AppendLine("}");


                if (data.ValueChangedOrchestrationMethod.HasValue())
                {
                    sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                }

                sb.PaddingCount--;
                sb.AppendLine("}}");
            }

           

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"labelText = {{{labelValue}}}");
            }

            sb.AppendLine("columns = {this." + data.TypeScriptMethodNameOfGetGridColumns + "(request)}");
            sb.AppendLine("multiColumn={true}");
            if (data.IsMultiSelect)
            {
                sb.AppendLine("multiSelect={true}");
            }
            else
            {
                sb.AppendLine("multiSelect={false}");
            }
            sb.AppendLine($"valueMemberPath=\"{valueMemberPath}\"");
            sb.AppendLine($"displayMemberPath=\"{displayMemberPath}\"");

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {"+ RenderHelper.GetJsValue(data.SizeInfo) +"}");
            }
            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }

        #endregion Public Methods
    }
}