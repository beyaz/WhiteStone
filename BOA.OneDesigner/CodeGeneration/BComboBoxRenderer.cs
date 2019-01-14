﻿using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BComboBoxRenderer
    {

        internal static string EvaluateMethodNameOfGridColumns(WriterContext writerContext, BComboBox data)
        {
            var last = data?.SelectedValueBindingPath?.SplitAndClear(".")?.LastOrDefault();

            return "getComboBoxColumnsOf" + last;
        }

      
      


        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BComboBox data)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine(methodName+"(request:any) : any[]");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const columns: any[] = [];");
            var isFirst = true;
            foreach (var bDataGridColumnInfo in data.DataGrid.Columns)
            {
                sb.AppendLine();
                sb.AppendLine("// "+bDataGridColumnInfo.BindingPath);

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

                var propertyInfo = writerContext.RequestIntellisenseData.FindPropertyInfoInCollectionFirstGenericArgumentType(data.DataGrid.DataSourceBindingPath,bDataGridColumnInfo.BindingPath);
                
                if (propertyInfo.IsDecimal||propertyInfo.IsDecimalNullable)
                {
                    sb.AppendLine("column.type = 'number';"); 
                    sb.AppendLine("column.numberFormat = 'M';"); 
                    
                }
                else if (propertyInfo.IsNumber)
                {
                    sb.AppendLine("column.type = 'number';");    
                }
                else if (propertyInfo.IsDate||propertyInfo.IsDateNullable)
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

            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            SnapNamingHelper.InitSnapName(data);

            
            var methodNameOfGridColumns = EvaluateMethodNameOfGridColumns(writerContext,data);
            writerContext.ClassBody.Add(EvaluateMethodBodyOfGridColumns(methodNameOfGridColumns,writerContext,data));

           

            var selectedValueBindingPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + data.SelectedValueBindingPath);
            var valueMemberPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + data.ValueMemberPath);

            sb.AppendLine($"<BComboBox  dataSource = {{{data.DataGrid.DataSourceBindingPathInTypeScript}}}");

            sb.AppendLine($"value={selectedValueBindingPath}");
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

            sb.PaddingCount--;
            sb.AppendLine("}}");


            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            
            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.Append($"labelText = {{{labelValue}}},");
            }


            sb.AppendLine("columns = {this."+methodNameOfGridColumns+"(request)}");
            sb.AppendLine("multiColumn={true}");
            sb.AppendLine("multiSelect={false}");
            sb.AppendLine($"valueMemberPath={{{data.ValueMemberPath}}}");
            sb.AppendLine($"displayMemberPath={{{data.DisplayMemberPath}}}");
            

            sb.AppendLine("context = {context}/>");
        }
        #endregion
    }
}