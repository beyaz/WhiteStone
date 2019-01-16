using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BDataGridRenderer
    {

        internal static string EvaluateMethodNameOfGridColumns(WriterContext writerContext, BDataGrid data)
        {
            var last = data?.DataSourceBindingPath?.SplitAndClear(".")?.LastOrDefault();

            return "getDataGridColumnsOf" + last;
        }

        internal static string EvaluateMethodNameOfGridRowSelectionChanged(WriterContext writerContext, BDataGrid data)
        {
            var last = data?.DataSourceBindingPath?.SplitAndClear(".")?.LastOrDefault();

            return "on"+last+"RowSelectionChanged";
        }

        internal static string EvaluateMethodBodyOfGridRowSelectionChanged(string methodName, WriterContext writerContext, BDataGrid data)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine(methodName+"()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            var fieldPath=TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value+data.SelectedRowDataBindingPath);
            var dataSourceBindingPath =TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value+data.DataSourceBindingPath);

            sb.AppendLine("const request:any = FormAssistant.getWindowRequest(this);");
            sb.AppendLine(fieldPath+$" = ({dataSourceBindingPath} as any[]).find(x => x.isSelected);");
            sb.AppendLine($"FormAssistant.executeWindowRequest(this,\"{data.RowSelectionChangedOrchestrationMethod}\");");

         
            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }


        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BDataGrid data)
        {
            var sb = new PaddedStringBuilder();
            sb.AppendLine(methodName+"(request:any) : any[]");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const columns: any[] = [];");
            var isFirst = true;
            foreach (var bDataGridColumnInfo in data.Columns)
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

                var propertyInfo = writerContext.RequestIntellisenseData.FindPropertyInfoInCollectionFirstGenericArgumentType(data.DataSourceBindingPath,bDataGridColumnInfo.BindingPath);

                if (propertyInfo != null)
                {
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
        public static void Write(WriterContext writerContext, BDataGrid data)
        {
            writerContext.Imports.Add("import { BDataGrid } from \"b-data-grid\"");

            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            SnapNamingHelper.InitSnapName(data);

            
            var methodNameOfGridColumns = EvaluateMethodNameOfGridColumns(writerContext,data);
            writerContext.AddClassBody(EvaluateMethodBodyOfGridColumns(methodNameOfGridColumns,writerContext,data));

            var rowSelectionChangedMethodName = EvaluateMethodNameOfGridRowSelectionChanged(writerContext,data);
            writerContext.AddClassBody(EvaluateMethodBodyOfGridRowSelectionChanged(rowSelectionChangedMethodName,writerContext,data));
            writerContext.ConstructorBody.Add($"this.{rowSelectionChangedMethodName} = this.{rowSelectionChangedMethodName}.bind(this);");



            sb.AppendLine($"<BDataGrid  dataSource = {{{data.DataSourceBindingPathInTypeScript}}}");

            sb.PaddingCount++;
            sb.AppendLine("selectable={'single'}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            



            sb.AppendLine("columns = {this."+methodNameOfGridColumns+"(request)}");
            sb.AppendLine("onRowSelectionChanged={this."+rowSelectionChangedMethodName+"}");

            sb.AppendLine("headerBarOptions={{");

            sb.PaddingCount++;

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.TitleInfo);
            if (labelValue != null)
            {
                sb.Append("showTitle:true,");
                sb.Append($"title = {{{labelValue}}},");
            }

            sb.AppendLine("show: true,");
            sb.AppendLine("showMoreOptions: true,");
            sb.AppendLine("showFiltering: true,");
            sb.AppendLine("showGrouping: true");
            sb.PaddingCount--;
            sb.AppendLine("}}");

            sb.AppendLine("context = {context}/>");
        }
        #endregion
    }
}